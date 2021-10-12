using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;

namespace Mountain.Protocol.Packet.In
{
    public class PacketPlayInClientSettings : IInboundPacket
    {
        public byte PacketId => Packets.In.Play.ClientSettings.PacketId;

        public string Locale { get; private set; }
        public byte ViewDistance { get; private set; }
        public ClientChatMode ChatMode { get; private set; }
        public bool ChatColorEnabled { get; private set; }
        public SkinRenderPreferences SkinParts { get; private set; }
        public bool RightMainHand { get; private set; }
        public bool DisableTextFiltering { get; private set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Locale = stream.ReadVarString();
            ViewDistance = stream.ReadByteSafe();
            ChatMode = stream.ReadEnumVarInt<ClientChatMode>();
            ChatColorEnabled = stream.ReadBool();
            SkinParts = new SkinRenderPreferences(stream.ReadByteSafe());
            RightMainHand = stream.ReadVarInt() == 1;
            DisableTextFiltering = stream.ReadBool();
        }

        public void Handle(IConnectionManager manager, IClientConnection clientConnection)
        {
            throw new NotImplementedException();
        }
    }
}
