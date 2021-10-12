using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.World
{
    public class Location : Position
    {
        public World World { get; set; }

        public Location(World world, double x, double y, double z) : base(x, y, z)
        {
            World = world;
            PosX = x;
            PosY = y;
            PosZ = z;
        }

        public new object Clone()
        {
            return new Location(World, PosX, PosY, PosZ);
        }
    }
}
