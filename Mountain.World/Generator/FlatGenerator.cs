using Mountain.Core.Block;
using Mountain.World.Biome;
using Mountain.World.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mountain.World.Generator
{
    public class FlatGenerator : IChunkGenerator
    {
        private readonly List<(int, BlockState)> layers;
        private readonly IBiomeType<IBiome> biome;

        public FlatGenerator(IEnumerable<(int, BlockState)> layers, IBiomeType<IBiome> biome)
        {
            this.layers = new List<(int, BlockState)>(layers);
            this.biome = biome;
        }

        public FlatGenerator(IEnumerable<KeyValuePair<int, BlockState>> layers, IBiomeType<IBiome> biome)
        {
            this.layers = new List<(int, BlockState)>(layers.Select(layer => { return (layer.Key, layer.Value); }));
            this.biome = biome;
        }

        public Chunk GenerateChunkFull(int x, int z)
        {
            var chunk = new Chunk(x, z);
            if (layers.Count < 1)
            {
                return chunk;
            }

            int currentHeight = World.WORLD_Y_MIN;

            foreach ((int layerHeight, BlockState block) in layers)
            {
                if (currentHeight + layerHeight > World.WORLD_Y_MAX)
                {
                    break;
                }

                chunk.FillBlocks(0, currentHeight, 0, 15, currentHeight + layerHeight, 15, block);
                currentHeight += layerHeight;
            }

            return chunk;
        }
    }
}
