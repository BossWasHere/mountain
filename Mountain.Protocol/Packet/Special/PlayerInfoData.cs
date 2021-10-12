using Mountain.Core;
using Mountain.Core.Chat;
using Mountain.Core.Enums;
using Mountain.Mojang;
using System.IO;

namespace Mountain.Protocol.Packet.Special
{
    public abstract class PlayerInfoData
    {
        public Uuid PlayerUuid { get; set; }

        public virtual void WriteToStream(Stream stream)
        {
            stream.WriteUuid(PlayerUuid);
        }

        public virtual void ReadFromStream(Stream stream)
        {
            PlayerUuid = stream.ReadUuid();
        }

        public class AddPlayerData : PlayerInfoData
        {
            public string Name { get; set; }
            public SignedProperty[] Properties { get; set; }
            public Gamemode Gamemode { get; set; }
            public int Ping { get; set; }
            public ChatMessage DisplayName { get; set; }

            public override void WriteToStream(Stream stream)
            {
                base.WriteToStream(stream);
                stream.WriteVarString(Name);
                stream.WriteVarInt(Properties.Length);
                foreach (SignedProperty property in Properties)
                {
                    stream.WriteVarString(property.Name);
                    stream.WriteVarString(property.Value);
                    if (property.Signature == null)
                    {
                        stream.WriteBool(false);
                    }
                    else
                    {
                        stream.WriteBool(true);
                        stream.WriteVarString(property.Signature);
                    }
                }

                stream.WriteEnumVarInt(Gamemode);
                stream.WriteVarInt(Ping);

                if (DisplayName == null)
                {
                    stream.WriteBool(false);
                }
                else
                {
                    stream.WriteBool(true);
                    DisplayName.WriteVarStringUtf8Bytes(stream);
                }
            }

            public override void ReadFromStream(Stream stream)
            {
                base.ReadFromStream(stream);
                Name = stream.ReadVarString();
                Properties = new SignedProperty[stream.ReadVarInt()];

                for (int i = 0; i < Properties.Length; i++)
                {
                    Properties[i] = new SignedProperty() { Name = stream.ReadVarString(), Value = stream.ReadVarString() };
                    if (stream.ReadBool())
                    {
                        Properties[i].Signature = stream.ReadVarString();
                    }
                }

                Gamemode = stream.ReadEnumVarInt<Gamemode>();
                Ping = stream.ReadVarInt();

                if (stream.ReadBool())
                {
                    var buf = new byte[stream.ReadVarInt()];
                    stream.Read(buf);
                    DisplayName = ChatMessage.DeserializeSafe(buf);
                }
                else
                {
                    DisplayName = null;
                }
            }
        }

        public class UpdateGamemodeData : PlayerInfoData
        {
            public Gamemode Gamemode { get; set; }

            public override void WriteToStream(Stream stream)
            {
                base.WriteToStream(stream);
                stream.WriteEnumVarInt(Gamemode);
            }

            public override void ReadFromStream(Stream stream)
            {
                base.ReadFromStream(stream);
                Gamemode = stream.ReadEnumVarInt<Gamemode>();
            }
        }

        public class UpdateLatencyData : PlayerInfoData
        {
            public int Ping { get; set; }

            public override void WriteToStream(Stream stream)
            {
                base.WriteToStream(stream);
                stream.WriteVarInt(Ping);
            }

            public override void ReadFromStream(Stream stream)
            {
                base.ReadFromStream(stream);
                Ping = stream.ReadVarInt();
            }
        }

        public class UpdateDisplayNameData : PlayerInfoData
        {
            public ChatMessage DisplayName { get; set; }

            public override void WriteToStream(Stream stream)
            {
                base.WriteToStream(stream);
                if (DisplayName == null)
                {
                    stream.WriteBool(false);
                }
                else
                {
                    stream.WriteBool(true);
                    DisplayName.WriteVarStringUtf8Bytes(stream);
                }
            }

            public override void ReadFromStream(Stream stream)
            {
                base.ReadFromStream(stream);
                if (stream.ReadBool())
                {
                    var buf = new byte[stream.ReadVarInt()];
                    stream.Read(buf);
                    DisplayName = ChatMessage.DeserializeSafe(buf);
                }
                else
                {
                    DisplayName = null;
                }
            }
        }

        public class RemovePlayerData : PlayerInfoData
        { }
    }
}
