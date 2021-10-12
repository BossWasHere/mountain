namespace Mountain.World.Settings
{
    public abstract class GeneratorSettings
    {
        public string Name { get; set; }
        public int WorldSize { get; set; }
        public long Seed { get; set; }
        public string ChunkGeneratorName { get; set; }
    }
}
