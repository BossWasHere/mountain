using Mountain.World.Level;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World.Generator
{
    public interface IChunkGenerator
    {
        Chunk GenerateChunkFull(int x, int z);
    }
}
