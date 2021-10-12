using Mountain.Core;

namespace Mountain.Mojang
{
    public struct SkinTextureData
    {
        public long Timestamp { get; set; }
        public Uuid Uuid { get; set; }
        public string Username { get; set; }
        public bool RequiresSignature { get; set; }
        public SkinUrlCache TextureUrls { get; set; }
    }
}
