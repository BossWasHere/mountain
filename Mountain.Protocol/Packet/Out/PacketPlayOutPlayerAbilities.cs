using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutPlayerAbilities : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.PlayerAbilities.PacketId;

        public bool Invulnerable { get; set; }
        public bool Flying { get; set; }
        public bool AllowFlying { get; set; }
        public bool InstantBreak { get; set; }
        public float FlySpeed { get; set; }
        public float FoVModifier { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            var flag = stream.ReadByteSafe();
            Invulnerable = (flag & 0x01) == 0x01;
            Flying = (flag & 0x02) == 0x02;
            AllowFlying = (flag & 0x04) == 0x04;
            InstantBreak = (flag & 0x08) == 0x08;

            FlySpeed = stream.ReadFloat();
            FoVModifier = stream.ReadFloat();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);

            byte flag = (byte)((Invulnerable ? 0x01 : 0x00) | (Flying ? 0x02 : 0x00) | (AllowFlying ? 0x04 : 0x00) | (InstantBreak ? 0x08 : 0x00));
            stream.WriteByte(flag);
            stream.WriteFloat(FlySpeed);
            stream.WriteFloat(FoVModifier);
        }
    }
}
