using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World
{
    public class Position : BlockPosition
    {
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }

        public int BlockPosX
        {
            get
            {
                return (int)Math.Floor(PosX);
            }
            set
            {
                PosX = value;
            }
        }
        public int BlockPosY
        {
            get
            {
                return (int)Math.Floor(PosY);
            }
            set
            {
                PosY = value;
            }
        }
        public int BlockPosZ
        {
            get
            {
                return (int)Math.Floor(PosZ);
            }
            set
            {
                PosZ = value;
            }
        }

        public Position()
        { }

        public Position(double x, double y, double z) 
        {
            PosX = x;
            PosY = y;
            PosZ = z;
        }

        public new object Clone()
        {
            return new Position(PosX, PosY, PosZ);
        }
    }
}
