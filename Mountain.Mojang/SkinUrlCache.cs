using System;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct SkinUrlCache
    {
        [JsonPropertyName("SKIN")]
        private UrlEntry SkinEntry { get; set; }
        [JsonPropertyName("CAPE")]
        private UrlEntry CapeEntry { get; set; }

        public string Skin
        {
            get => SkinEntry?.Url;
            set
            {
                if (SkinEntry == null) SkinEntry = new UrlEntry();
                SkinEntry.Url = value;
            }
        }

        public bool HasCustomSkin => SkinEntry != null;
        public bool IsSlimModel => "slim".Equals(SkinEntry?.Metadata?.ModelType, StringComparison.InvariantCultureIgnoreCase);

        public string Cape
        {
            get => CapeEntry?.Url;
            set
            {
                if (CapeEntry == null) CapeEntry = new UrlEntry();
                CapeEntry.Url = value;
            }
        }

        private class UrlEntry
        {
            [JsonPropertyName("url")]
            internal string Url { get; set; }
            [JsonPropertyName("metadata")]
            internal AdditionalMeta Metadata { get; set; }
        }

        private class AdditionalMeta
        {
            [JsonPropertyName("model")]
            internal string ModelType { get; set; }
        }
    }
}
