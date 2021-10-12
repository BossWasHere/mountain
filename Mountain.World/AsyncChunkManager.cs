using Mountain.Core;
using Mountain.World.Generator;
using Mountain.World.Level;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Refer to https://devblogs.microsoft.com/premier-developer/the-danger-of-taskcompletionsourcet-class/

namespace Mountain.World
{
    public sealed class AsyncChunkManager : IDisposable
    {
        private readonly IChunkGenerator generator;
        private readonly WorldFileSystemService wfss;
        private ConcurrentDictionary<(int, int), Chunk> chunks;
        private BlockingCollection<(int, int, TaskCompletionSource<Chunk>)> chunkLoaderQueue;
        private ConcurrentDictionary<TaskCompletionSource<Chunk>, int> chunkLoaderInProgress;
        private Task[] loaderWorkers;

        public int LoaderThreads { get; }

        private TaskFactory workerFactory;
        private CancellationTokenSource workerCancellationToken;
        private bool workersEnabled = false;
        private bool doWork = false;
        private bool disposed = false;

        public AsyncChunkManager(IChunkGenerator generator, WorldFileSystemService wfss, int loaderThreads, bool autoStart = true)
        {
            this.generator = generator;
            this.wfss = wfss;
            LoaderThreads = Math.Max(loaderThreads, 1);
            chunks = new ConcurrentDictionary<(int, int), Chunk>();
            chunkLoaderQueue = new BlockingCollection<(int, int, TaskCompletionSource<Chunk>)>(new ConcurrentQueue<(int, int, TaskCompletionSource<Chunk>)>());
            chunkLoaderInProgress = new ConcurrentDictionary<TaskCompletionSource<Chunk>, int>();
            workerCancellationToken = new CancellationTokenSource();
            workerFactory = new TaskFactory(workerCancellationToken.Token);

            if (autoStart) StartWorkers();
        }

        public void StartWorkers()
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));
            if (workersEnabled) return;

            workersEnabled = true;
            doWork = true;

            loaderWorkers ??= new Task[LoaderThreads];

            for (var i = 0; i < LoaderThreads; i++)
            {
                loaderWorkers[i] = workerFactory.StartNew(() => ChunkQueueConsumerWorker(i)).SetTaskName("Chunk Generator " + i);
            }
        }

        public async Task StopWorkers(bool awaitCurrentChunks)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));
            if (!workersEnabled) return;

            doWork = false;

            if (awaitCurrentChunks)
            {
                await Task.WhenAll(loaderWorkers).ConfigureAwait(false);
            }
            workerCancellationToken.Cancel();

            foreach (var inProgress in chunkLoaderInProgress)
            {
                inProgress.Key.TrySetCanceled();
            }

            // TODO danger? should we drain queue and report task cancelled?

            workersEnabled = false;
        }

        private void ChunkQueueConsumerWorker(int workerId)
        {
            while (doWork)
            {
                (int x, int z, TaskCompletionSource<Chunk> tcs) = chunkLoaderQueue.Take();
                if (tcs != null) chunkLoaderInProgress.TryAdd(tcs, workerId);
                Chunk chunk = generator.GenerateChunkFull(x, z);

                chunks.TryAdd((x, z), chunk);

                if (tcs != null)
                {
                    tcs.SetResult(chunk);
                    chunkLoaderInProgress.TryRemove(tcs, out _);
                }
            }
        }

        public Task<Chunk> LoadChunk(int x, int z)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            if (chunks.TryGetValue((x, z), out var chunk)) return Task.FromResult(chunk);

            TaskCompletionSource<Chunk> tcs = new TaskCompletionSource<Chunk>(TaskContinuationOptions.RunContinuationsAsynchronously);

            chunkLoaderQueue.Add((x, z, tcs));

            return tcs.Task;
        }

        public Task LoadChunks((int, int)[] chunks)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            TaskCompletionSource<Chunk> tcs = new TaskCompletionSource<Chunk>(TaskContinuationOptions.RunContinuationsAsynchronously);

            for (int i = 0; i < chunks.Length; i++)
            {
                if (this.chunks.ContainsKey(chunks[i]))
                {
                    continue;
                }

                chunkLoaderQueue.Add((chunks[i].Item1, chunks[i].Item2, i == chunks.Length - 1 ? tcs : null));
            }

            return tcs.Task;
        }

        public bool EnqueueLoadChunk(int x, int z)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            if (chunks.ContainsKey((x, z))) return false;

            chunkLoaderQueue.Add((x, z, null));

            return true;
        }

        public int EnqueueLoadChunks((int, int)[] chunks)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            int existing = 0;
            for (int i = 0; i < chunks.Length; i++)
            {
                if (this.chunks.ContainsKey(chunks[i]))
                {
                    existing++;
                    continue;
                }

                chunkLoaderQueue.Add((chunks[i].Item1, chunks[i].Item2, null));
            }

            return existing;
        }

        public bool UnloadChunk(int x, int z, bool save)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            if (chunks.TryRemove((x, z), out Chunk chunk))
            {
                if (save)
                {
                    _ = wfss.SaveChunk(chunk);
                }

                return true;
            }

            return false;
        }

        public int UnloadChunks((int, int, bool)[] chunks)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            int unloaded = 0;
            foreach (var chunkPos in chunks)
            {
                if (this.chunks.TryRemove((chunkPos.Item1, chunkPos.Item2), out Chunk chunk))
                {
                    if (chunkPos.Item3)
                    {
                        _ = wfss.SaveChunk(chunk);
                    }

                    unloaded++;
                }
            }

            return unloaded;
        }

        public int UnloadChunks((int, int)[] chunks, bool save)
        {
            if (disposed) throw new ObjectDisposedException(nameof(AsyncChunkManager));

            int unloaded = 0;
            foreach (var chunkPos in chunks)
            {
                if (this.chunks.TryRemove((chunkPos.Item1, chunkPos.Item2), out Chunk chunk))
                {
                    if (save)
                    {
                        _ = wfss.SaveChunk(chunk);
                    }

                    unloaded++;
                }
            }

            return unloaded;
        }

        //public async Task SaveChunk(int x, int z)
        //{
        //    throw new NotImplementedException();
        //}

        ~AsyncChunkManager() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            disposed = true;

            if (disposing)
            {
                workerCancellationToken?.Cancel();
                chunks?.Clear();
                // TODO maybe this?
                //chunkLoaderQueue?.CompleteAdding();
                chunkLoaderQueue?.Dispose();
                workerCancellationToken?.Dispose();
            }

            chunks = null;
            chunkLoaderQueue = null;
            workerFactory = null;
            workerCancellationToken = null;
        }
    }
}
