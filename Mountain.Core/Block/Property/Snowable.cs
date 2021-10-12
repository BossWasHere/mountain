using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block.Property
{
    public struct Snowable : IBlockProperty
    {
        public static readonly Snowable Default = new Snowable(false);
        public static readonly Snowable Snowy = new Snowable(true);

        public bool IsSnowy { get; }

        public Snowable(bool isSnowy)
        {
            IsSnowy = isSnowy;
        }

        public int GetStateNumber()
        {
            return IsSnowy ? 0 : 1;
        }

        //public static Snowable GetDefaultState()
        //{
        //    return Default;
        //}

        //public static Snowable[] GetStates()
        //{
        //    return new Snowable[] { Default, Snowy };
        //}
    }
}
