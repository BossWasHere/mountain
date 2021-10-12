using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block.Property
{
    public struct Sapling : IBlockProperty
    {
        public static readonly Sapling First = new Sapling(true);
        public static readonly Sapling Second = new Sapling(false);

        public bool FirstStage { get; }

        public Sapling(bool isFirst)
        {
            FirstStage = isFirst;
        }

        public int GetStateNumber()
        {
            return FirstStage ? 0 : 1;
        }

        //public static Sapling GetDefaultState()
        //{
        //    return First;
        //}

        //public static Sapling[] GetStates()
        //{
        //    return new Sapling[] { First, Second };
        //}
    }
}
