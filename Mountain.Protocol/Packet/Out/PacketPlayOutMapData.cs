using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Core.Item.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutMapData : IOutboundPacket
    {
        public byte PacketId => Packets.Out.Play.MapData.PacketId;

        public int MapId { get; set; }
        public byte Scale { get; set; }
        public bool Locked { get; set; }
        //public bool TrackingPosition {  get; set; }
        public Icon[] Icons { get; set; }
        public byte Columns { get; set; }
        public byte Rows { get; set; }
        public byte X { get; set; }
        public byte Z { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteVarInt(MapId);
            stream.WriteByte(Scale);
            stream.WriteBool(Locked);
            bool hasIcons = Icons != null && Icons.Length > 0;
            stream.WriteBool(hasIcons);
            if (hasIcons)
            {
                foreach (Icon icon in Icons)
                {
                    stream.WriteEnumVarInt(icon.Type);
                    stream.WriteByte(icon.X);
                    stream.WriteByte(icon.Z);
                    stream.WriteByte(icon.Direction);
                    bool hasDisplayName = icon.DisplayName != null;
                    stream.WriteBool(hasDisplayName);
                    if (hasDisplayName)
                    {
                        icon.DisplayName.WriteVarStringUtf8Bytes(stream);
                    }
                }
            }
            stream.WriteByte(Columns);
            if (Columns > 0)
            {
                stream.WriteByte(Rows);
                stream.WriteByte(X);
                stream.WriteByte(Z);
                stream.WriteVarIntPrefixedByteArray(Data);
            }
        }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            //if (stream.ReadBool())
            //{
            //    Icons = new Icon[stream.ReadVarInt()];
            //    for (int i = 0; i < Icons.Length; i++)
            //    {
            //        Icons[i] = new Icon()
            //        {
            //            Type = stream.ReadEnumVarInt<MapIconType>(),
            //            X = stream.ReadByteSafe(),
            //            Z = stream.ReadByteSafe(),
            //            Direction = stream.ReadByteSafe(),
            //            Message = ...
            //        };
            //    }
            //}
        }
    }
}
