﻿using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct SignedProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("signature")]
        public string Signature { get; set; }
    }
}
