using Mountain.Core.Enums;
using Mountain.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Block.Property
{
    public struct AxisAligned : IBlockProperty
    {
        public Axis Axis { get; }

        public AxisAligned(Axis axis)
        {
            Axis = axis;
        }

        public int GetStateNumber()
        {
            return Axis switch
            {
                Axis.X => 0,
                Axis.Y => 1,
                _ => 2
            };
        }
    }
}
