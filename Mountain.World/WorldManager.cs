using Mountain.World.Env;
using Mountain.World.Generator;
using Mountain.World.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mountain.World
{
    public class WorldManager
    {
        public string WorldDirectory { get; }
        public Dictionary<string, IChunkGeneratorProvider> Generators { get; }
        private readonly ConcurrentDictionary<string, World> worlds;

        public WorldManager(string worldDirectory)
        {
            WorldDirectory = worldDirectory;
            Generators = new Dictionary<string, IChunkGeneratorProvider>(AssemblyData.GetBuiltinGeneratorProviders());
            worlds = new ConcurrentDictionary<string, World>();
        }

        public async Task<World> LoadWorld(string name)
        {
            if (worlds.TryGetValue(name, out World world))
            {
                return world;
            }

            string worldDir = Path.Combine(WorldDirectory, name);
            if (!Directory.Exists(worldDir))
            {
                return null;
            }

            // TODO actually load worlds
            return null;
        }

        public async Task<World> LoadDummyWorld()
        {
            var world = World.Create(this, new DummyWorldSettings());
            if (!worlds.TryAdd("world", world))
            {
                return null;
            }
            world.StartWorld();
            await world.LoadSpawnChunksAsync(10);
            return world;
        }

        public async Task<World> LoadOrCreateWorld(string name)
        {
            // TODO remove this bit
            return await LoadDummyWorld();
        }

        public async Task<bool> DeleteWorld(string name)
        {
            throw new NotImplementedException();
        }

        public World GetLoadedWorld(string name)
        {
            return worlds.TryGetValue(name, out World world) ? world : null;
        }

        public IChunkGenerator CreateGenerator(string generatorName)
        {
            return Generators.TryGetValue(generatorName, out IChunkGeneratorProvider provider) ? provider.NewInstance() : null;
        }
    }
}
