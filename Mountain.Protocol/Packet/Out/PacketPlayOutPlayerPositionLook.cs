using Mountain.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutPlayerPositionLook : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.PlayerPositionLook.PacketId;

        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool IsRelativeXPosition { get; set; }
        public bool IsRelativeYPosition { get; set; }
        public bool IsRelativeZPosition { get; set; }
        public bool IsRelativePitch { get; set; }
        public bool IsRelativeYaw { get; set; }
        public int TeleportId { get; set; }
        public bool ShouldDismountVehicle { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            PosX = stream.ReadDouble();
            PosY = stream.ReadDouble();
            PosZ = stream.ReadDouble();
            Yaw = stream.ReadFloat();
            Pitch = stream.ReadFloat();

            var flag = stream.ReadByteSafe();
            IsRelativeXPosition = (flag & 0x01) == 0x01;
            IsRelativeYPosition = (flag & 0x02) == 0x02;
            IsRelativeZPosition = (flag & 0x04) == 0x04;
            IsRelativePitch = (flag & 0x08) == 0x08;
            IsRelativeYaw = (flag & 0x10) == 0x10;

            TeleportId = stream.ReadVarInt();
            ShouldDismountVehicle = stream.ReadBool();
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteDouble(PosX);
            stream.WriteDouble(PosY);
            stream.WriteDouble(PosZ);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);

            var flag = (byte)((IsRelativeXPosition ? 0x01 : 0x00) | (IsRelativeYPosition ? 0x02 : 0x00) | (IsRelativeZPosition ? 0x04 : 0x00) | (IsRelativePitch ? 0x08 : 0x00) | (IsRelativeYaw ? 0x10 : 0x00));
            stream.WriteByte(flag);
            stream.WriteVarInt(TeleportId);
            stream.WriteBool(ShouldDismountVehicle);
        }
    }
}
