using Mountain.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block.Property
{
    public struct Leaf : IBlockProperty
    {
        public int Distance { get; }
        public bool IsPersistent { get; }

        public Leaf(int distance, bool persistent)
        {
            Distance = distance.Clamp(1, 7);
            IsPersistent = persistent;
        }

        public int GetStateNumber()
        {
            return (Distance - 1) * 2 + (IsPersistent ? 0 : 1);
        }

        //public static Leaf GetDefaultState()
        //{
        //    return new Leaf(1, false);
        //}

        //public static Leaf[] GetStates()
        //{
        //    return new Leaf[]
        //    {
        //        new Leaf(1, false),
        //        new Leaf(2, false),
        //        new Leaf(3, false),
        //        new Leaf(4, false),
        //        new Leaf(5, false),
        //        new Leaf(6, false),
        //        new Leaf(7, false),
        //        new Leaf(1, true),
        //        new Leaf(2, true),
        //        new Leaf(3, true),
        //        new Leaf(4, true),
        //        new Leaf(5, true),
        //        new Leaf(6, true),
        //        new Leaf(7, true),
        //    };
        //}
    }
}
