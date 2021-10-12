using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World
{
    public class BlockPosition : ICloneable
    {
        public int BlockPositionX { get; set; }
        public int BlockPositionY { get; set; }
        public int BlockPositionZ { get; set; }

        public BlockPosition()
        { }

        public BlockPosition(ulong xyz) : this((int)(xyz >> 38), (int)(xyz & 0xFFF), (int)(xyz << 26 >> 38))
        { }

        public BlockPosition(int x, int y, int z)
        {
            BlockPositionX = x;
            BlockPositionY = y;
            BlockPositionZ = z;
        }

        public object Clone()
        {
            return new BlockPosition(BlockPositionX, BlockPositionY, BlockPositionZ);
        }

        public ulong ToUInt64() => (ulong)(BlockPositionX & 0x3FFFFFF) << 38 | (ulong)(BlockPositionZ & 0x3FFFFFF) << 12 | (ulong)BlockPositionY & 0xFFF;
    }
}
