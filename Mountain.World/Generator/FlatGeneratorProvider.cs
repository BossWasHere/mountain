using Mountain.Core;
using Mountain.Core.Block;
using Mountain.World.Biome;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Generator
{
    public class FlatGeneratorProvider : IChunkGeneratorProvider
    {
        public string GeneratorName => "flat";

        public IChunkGenerator NewInstance()
        {
            // TODO not always this please remove and refactor everything about this
            List<(int, BlockState)> layers = new List<(int, BlockState)>();
            layers.Add((1, new BlockState(Materials.Bedrock)));
            layers.Add((3, new BlockState(Materials.Dirt)));
            layers.Add((1, new BlockState(Materials.GrassBlock)));

            return new FlatGenerator(layers, Biomes.Plains);
        }
    }
}
