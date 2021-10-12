using Mountain.Core;
using Mountain.Core.Enums;
using Mountain.Protocol.Packet.Special;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.Packet.Out
{
    public class PacketPlayOutPlayerInfo : IOutboundPacket, IPacketDeserializable
    {
        public byte PacketId => Packets.Out.Play.PlayerInfo.PacketId;

        public PlayerInfoAction Action { get; set; }
        public PlayerInfoData[] Data { get; set; }

        public void ReadFromStream(Stream stream, int lengthHint)
        {
            Action = stream.ReadEnumVarInt<PlayerInfoAction>();
            Data = new PlayerInfoData[stream.ReadVarInt()];

            for (int i = 0; i < Data.Length; i++)
            {
                PlayerInfoData item = null;
                switch (Action)
                {
                    case PlayerInfoAction.AddPlayer:
                        item = new PlayerInfoData.AddPlayerData();
                        break;
                    case PlayerInfoAction.UpdateGamemode:
                        item = new PlayerInfoData.UpdateGamemodeData();
                        break;
                    case PlayerInfoAction.UpdateLatency:
                        item = new PlayerInfoData.UpdateLatencyData();
                        break;
                    case PlayerInfoAction.UpdateDisplayName:
                        item = new PlayerInfoData.UpdateDisplayNameData();
                        break;
                    case PlayerInfoAction.RemovePlayer:
                        item = new PlayerInfoData.RemovePlayerData();
                        break;
                }

                item.ReadFromStream(stream);
                Data[i] = item;
            }
        }

        public void WriteToStream(Stream stream)
        {
            stream.WriteByte(PacketId);
            stream.WriteEnumVarInt(Action);
            stream.WriteVarInt(Data.Length);

            foreach (PlayerInfoData pid in Data)
            {
                pid.WriteToStream(stream);
            }
        }
    }
}
