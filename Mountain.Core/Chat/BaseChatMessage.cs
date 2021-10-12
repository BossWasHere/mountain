using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public abstract class BaseChatMessage
    {

        protected static readonly JsonSerializerOptions sOpts = new JsonSerializerOptions() { IgnoreNullValues = true };

        [JsonPropertyName("color")]
        public ResolvableChatColor Color { get; set; }
        [JsonPropertyName("bold")]
        public bool? Bold { get; set; }
        [JsonPropertyName("italic")]
        public bool? Italic { get; set; }
        [JsonPropertyName("underlined")]
        public bool? Underlined { get; set; }
        [JsonPropertyName("strikethrough")]
        public bool? Strikethrough { get; set; }
        [JsonPropertyName("obfuscated")]
        public bool? Obfuscated { get; set; }

        [JsonPropertyName("insertion")]
        public string Insertion { get; set; }
        [JsonPropertyName("font")]
        public string Font { get; set; }
        [JsonPropertyName("clickEvent")]
        public InteractEvent ClickEvent { get; set; }
        [JsonPropertyName("hoverEvent")]
        public InteractEvent HoverEvent { get; set; }
        [JsonIgnore]
        public BaseChatMessage[] Extra { get; set; }
        [JsonPropertyName("extra")]
        public object[] DynamicTypeExtra => Extra;

        public virtual byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(BaseChatMessage));
        }

        public virtual byte[] SerializeToVarStringUtf8Bytes()
        {
            var bytes = SerializeToUtf8Bytes();
            var lenBytes = DataTypes.WriteVarInt(bytes.Length);
            return DataTypes.Join(lenBytes, bytes);
        }

        public virtual void WriteUtf8Bytes(Stream stream)
        {
            stream.Write(SerializeToUtf8Bytes());
        }

        public virtual void WriteVarStringUtf8Bytes(Stream stream)
        {
            var bytes = SerializeToUtf8Bytes();
            stream.WriteVarInt(bytes.Length);
            stream.Write(bytes);
        }

        public virtual string Serialize()
        {
            return Serialize(typeof(BaseChatMessage));
        }

        protected byte[] SerializeToUtf8Bytes(Type type)
        {
            return JsonSerializer.SerializeToUtf8Bytes(this, type, sOpts);
        }

        protected string Serialize(Type type)
        {
            return JsonSerializer.Serialize(this, type, sOpts);
        }
    }
}
