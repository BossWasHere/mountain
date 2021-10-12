using Mountain.World.Generator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World
{
    public struct ChunkLoaderOptions
    {
        public ChunkGenerationStage InitialGenerationMode { get; set; }
        public bool KeepLoaded { get; set; }

    }
}
