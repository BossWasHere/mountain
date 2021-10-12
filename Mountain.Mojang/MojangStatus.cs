using Mountain.Core.Enums;
using System.Text;
using System.Text.Json.Serialization;

namespace Mountain.Mojang
{
    public struct MojangStatus
    {
        [JsonPropertyName("minecraft.net")]
        public ColorCodeStatus MinecraftNet { get; set; }
        [JsonPropertyName("session.minecraft.net")]
        public ColorCodeStatus SessionMinecraftNet { get; set; }
        [JsonPropertyName("account.mojang.com")]
        public ColorCodeStatus AccountMojangCom { get; set; }
        [JsonPropertyName("auth.mojang.com")]
        public ColorCodeStatus AuthMojangCom { get; set; }
        [JsonPropertyName("skins.minecraft.net")]
        public ColorCodeStatus SkinsMinecraftNet { get; set; }
        [JsonPropertyName("authserver.mojang.com")]
        public ColorCodeStatus AuthServerMojangCom { get; set; }
        [JsonPropertyName("sessionserver.mojang.com")]
        public ColorCodeStatus SessionServerMojangCom { get; set; }
        [JsonPropertyName("api.mojang.com")]
        public ColorCodeStatus ApiMojangCom { get; set; }
        [JsonPropertyName("textures.minecraft.net")]
        public ColorCodeStatus TexturesMinecraftNet { get; set; }
        [JsonPropertyName("mojang.com")]
        public ColorCodeStatus MojangCom { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder("MojangStatus;");
            builder.Append("MinecraftNet=").Append(MinecraftNet);
            builder.Append(",SessionMinecraftNet=").Append(SessionMinecraftNet);
            builder.Append(",AccountMojangCom=").Append(AccountMojangCom);
            builder.Append(",AuthMojangCom=").Append(AuthMojangCom);
            builder.Append(",SkinsMinecraftNet=").Append(SkinsMinecraftNet);
            builder.Append(",AuthServerMojangCom=").Append(AuthServerMojangCom);
            builder.Append(",SessionServerMojangCom=").Append(SessionServerMojangCom);
            builder.Append(",ApiMojangCom=").Append(ApiMojangCom);
            builder.Append(",TexturesMinecraftNet=").Append(TexturesMinecraftNet);
            builder.Append(",MojangCom=").Append(MojangCom);

            return builder.ToString();
        }
    }
}
