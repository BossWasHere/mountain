using Mountain.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block.Property
{
    public struct Liquid : IBlockProperty
    {
        public int Level { get; }

        public Liquid(int level)
        {
            Level = level.Clamp(0, 15);
        }

        public int GetStateNumber()
        {
            return Level - 1;
        }
    }
}
