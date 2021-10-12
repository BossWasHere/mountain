using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Generator
{
    public enum ChunkGenerationStage
    {
        Empty,
        StructureStarts,
        StructureReferences,
        Biomes,
        Noise,
        Surface,
        Carvers,
        LiquidCarvers,
        Features,
        Light,
        Spawn,
        Heightmaps,
        Full
    }
}
