using Mountain.Core;
using Mountain.Core.Block;
using System;
using System.Collections.Generic;
using System.Text;

using static Mountain.Core.Utils.MathUtils;

namespace Mountain.World.Level
{
    public class Chunk
    {
        public int ChunkX { get; }
        public int ChunkY { get; }

        public short[][] Data { get; }

        public Chunk(int x, int y)
        {
            ChunkX = x;
            ChunkY = y;
            Data = new short[16][];
        }

        public BlockStateBase GetBlock(int x, int y, int z)
        {
            ValidateCoords(x, y, z);
            int section = y >> 4;
            short[] subchunk = Data[section];

            if (subchunk == null)
            {
                BlockStateBase.FromInt(0);
            }
            return BlockStateBase.FromInt(subchunk[x + (z << 4) + (y << 8)]);
        }

        public void SetBlock(int x, int y, int z, BlockStateBase blockState)
        {
            ValidateCoords(x, y, z);

            short[] subchunk = GetSubchunk(y);

            subchunk[x + (z << 4) + (y << 8)] = blockState.GetId();
        }

        public void FillBlocks(int x0, int y0, int z0, int x1, int y1, int z1, BlockStateBase blockState)
        {
            ValidateCoords(x0, y0, z0, "0");
            ValidateCoords(x1, y1, z1, "1");

            MinMax(ref x0, ref x1);
            MinMax(ref y0, ref y1);
            MinMax(ref z0, ref z1);

            short blockId = blockState.GetId();

            for (int y = y0; y < y1; y++)
            {
                short[] subchunk = GetSubchunk(y);
                for (int z = z0; z < z1; z++)
                {
                    for (int x = x0; x < x1; x++)
                    {
                        subchunk[x + (z << 4) + (y << 8)] = blockId;
                    }
                }
            }
        }

        private short[] GetSubchunk(int y)
        {
            int section = y >> 4;

            short[] subchunk;
            if (Data[section] == null)
            {
                subchunk = new short[65536];
                Data[section] = subchunk;
            }
            else
            {
                subchunk = Data[section];
            }
            return subchunk;
        }

        private void ValidateCoords(int x, int y, int z, string opt = null)
        {
            if (x < 0 || x > 15) throw new ArgumentOutOfRangeException(nameof(x) + opt == null ? "" : opt);
            if (y < 0 || y > 255) throw new ArgumentOutOfRangeException(nameof(y) + opt == null ? "" : opt);
            if (z < 0 || z > 15) throw new ArgumentOutOfRangeException(nameof(z) + opt == null ? "" : opt);
        }
    }
}
