using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Settings
{
    public class DummyWorldSettings : GeneratorSettings
    {
        public DummyWorldSettings()
        {
            Name = "world";
            WorldSize = World.MAX_WORLD_SIZE;
            Seed = 0;
            ChunkGeneratorName = "flat";
        }
    }
}
