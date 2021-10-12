using Mountain.World.Generator;
using Mountain.World.Level;
using Mountain.World.Settings;
using System;
using System.Threading.Tasks;

namespace Mountain.World
{
    public class World
    {
        public const int MAX_WORLD_SIZE = 29999984;
        public const int WORLD_Y_MIN = 0;
        public const int WORLD_Y_MAX = 255;
        public string Name { get; }
        public int WorldSize { get; }
        public long Seed { get; }
        public bool Ready { get; private set; }

        public IChunkGenerator ChunkGenerator { get; }
        public WorldFileSystemService LevelFS { get; }
        public AsyncChunkManager ChunkManager { get; }

        internal World(string name, int worldSize, long seed, IChunkGenerator chunkGenerator)
        {
            Name = name;
            WorldSize = worldSize;
            Seed = seed;
            ChunkGenerator = chunkGenerator;
            LevelFS = new WorldFileSystemService();
            ChunkManager = new AsyncChunkManager(ChunkGenerator, LevelFS, 4, false);
        }

        public void StartWorld()
        {
            ChunkManager.StartWorkers();
        }

        public async Task LoadSpawnChunksAsync(int radius)
        {
            (int, int)[] chunks = new (int, int)[radius*radius];

            int i = 0;
            int start = 0 - radius >> 1;
            int end = start + radius;
            for (int x = start; x < end; x++)
            {
                for (int z = start; z < end; z++)
                {
                    chunks[i] = (x, z);
                    i++;
                }
            }

            await ChunkManager.LoadChunks(chunks);
        }

        public static World Load()
        {
            throw new NotImplementedException();
        }

        public static World Create(WorldManager manager, GeneratorSettings settings)
        {
            return new World(settings.Name, settings.WorldSize > MAX_WORLD_SIZE ? MAX_WORLD_SIZE : settings.WorldSize, settings.Seed, manager.CreateGenerator(settings.ChunkGeneratorName));
        }

        public static long NewRandomSeed()
        {
            var b = new byte[8];
            new Random().NextBytes(b);
            return BitConverter.ToInt64(b);
        }
    }
}
