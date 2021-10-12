using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Protocol.Packet
{
    public struct PacketReadData
    {
        public IPacketDeserializable Packet { get; }
        public DeserializeState DeserializeState { get; }
        public bool IsCompressed { get; }
        public int PackedIdRead { get; }

        public PacketReadData(IPacketDeserializable packet, DeserializeState deserializeState, bool isCompressed, int packetId)
        {
            Packet = packet;
            DeserializeState = deserializeState;
            IsCompressed = isCompressed;
            PackedIdRead = packetId;
        }
    }
}
