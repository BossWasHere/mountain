namespace Mountain.World
{
    public class BlockLocation : BlockPosition
    {
        public World World { get; set; }

        public BlockLocation(World world, int x, int y, int z)
        {
            World = world;
            BlockPositionX = x;
            BlockPositionY = y;
            BlockPositionZ = z;
        }

        public new object Clone()
        {
            return new BlockLocation(World, BlockPositionX, BlockPositionY, BlockPositionZ);
        }
    }
}
