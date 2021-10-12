using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet
{
    public delegate T ByteReadDelegate<T>(byte[] bytes, out int length, int offset = 0);

    [Obsolete("Use streams instead")]
    public class PacketDeserializer
    {
        public const int COMPRESSION_BUF_READER = 2048;

        public int Length { get; private set; }
        public int InitialLength { get; private set; }
        public int RemainingBytes { get => Length - currentOffset; }
        public byte PacketId { get; private set; }

        public IPacketDeserializable Packet { get; private set; }

        private byte[] data;
        private int currentOffset = 0;

        public PacketDeserializer()
        { }

        public DeserializeState Deserialize(byte[] buffer, int offset, ConnectionState state, bool compressed = false)
        {
            var ls = LoadPacketBytes(buffer, offset, compressed);
            if (ls != DeserializeState.Done) return ls;

            // Get the correct packet depending on socket state
            IPacketType<IInboundPacket> packetType = Packets.GetInboundType(PacketId, state);

            // If the packet doesn't exist for us, discard it
            if (packetType == null) return DeserializeState.UnknownPacket;

            Packet = packetType.GetBase();
            try
            {
                //Temporary before removal:
                //REMOVE: Does this even work now?
                var stream = new MemoryStream(data);
                stream.ReadByte();

                Packet.ReadFromStream(stream, data.Length - 1);
            }
            catch
            {
                return DeserializeState.BadData;
            }
            
            return DeserializeState.Done;
        }

        public DeserializeState DeserializeOutbound(byte[] buffer, int offset, ConnectionState state, bool compressed = false)
        {
            var ls = LoadPacketBytes(buffer, offset, compressed);
            if (ls != DeserializeState.Done) return ls;

            // Get the correct packet depending on socket state
            IPacketType<IOutboundPacket> packetType = Packets.GetOutboundType(PacketId, state);

            // If the packet doesn't exist for us, discard it
            if (packetType == null) return DeserializeState.UnknownPacket;

            var b = packetType.GetBase();

            if (b is IPacketDeserializable pd)
            {
                Packet = pd;
                try
                {
                    //Temporary before removal:
                    //REMOVE: Does this even work now?
                    var stream = new MemoryStream(data);
                    stream.ReadByte();

                    Packet.ReadFromStream(stream, data.Length - 1);
                }
                catch
                {
                    //Packet = null;
                    return DeserializeState.BadData;
                }

                return DeserializeState.Done;
            }

            return DeserializeState.UnknownPacket;
        }

        private DeserializeState LoadPacketBytes(byte[] buffer, int offset, bool compressed = false)
        {
            // Where to start reading from
            currentOffset = offset;

            // How many bytes to process from the buffer / the packet length
            Length = DataTypes.ReadVarInt(buffer, out int lengthOffset, currentOffset);

            // How many bytes in total
            InitialLength = Length + lengthOffset;

            // Packet is longer than expected, discard
            if (Length + currentOffset > buffer.Length) return DeserializeState.TooLong;

            // Packet should be at least 1 byte packet ID, discard
            if (Length < 1) return DeserializeState.TooShort;

            // Update the current offset to after packet length int
            currentOffset += lengthOffset;

            if (compressed)
            {

                int uncompressedLength = DataTypes.ReadVarInt(buffer, out int uncompressedLengthOffset, currentOffset);
                if (uncompressedLength == 0)
                {
                    // This packet isn't actually compressed, skip compression byte
                    compressed = false;
                    Length--;
                    currentOffset++;
                }
                else
                {
                    // The packet length = length of (uncompressed length) + length of (compressed data)
                    int compressedLength = Length - uncompressedLengthOffset;
                    currentOffset += uncompressedLengthOffset;

                    using var cms = new MemoryStream(buffer, currentOffset, compressedLength);
                    using var iis = new InflaterInputStream(cms);
                    //if (iis.Length > int.MaxValue) throw new InvalidDataException("Too many bytes");

                    var infs = new MemoryStream();
                    /*var tbuf = new byte[COMPRESSION_BUF_READER];
                    int tpos = 0;

                    while (true)
                    {
                        int numRead = iis.Read(tbuf, tpos, COMPRESSION_BUF_READER);
                        iis.Cop
                        if (numRead <= 0)
                        {
                            break;
                        }
                        infs.Write(tbuf, 0, numRead);
                        tpos += numRead;
                    }
                    */
                    iis.CopyTo(infs);

                    // Update the length
                    Length = (int)infs.Length;

                    data = infs.ToArray();
                }
            }
            if (!compressed)
            {
                // Copy relevant data
                data = new byte[Length];
                for (int i = 0; i < Length; i++)
                {
                    data[i] = buffer[currentOffset + i];
                }
            }

            // Make the offset relative to the new data array
            currentOffset = 0;

            // The (supposed) packet ID, the next byte
            PacketId = data[currentOffset++];

            return DeserializeState.Done;
        }

        public void Handle(IConnectionManager manager, IClientConnection client)
        {
            if (Packet is IInboundPacket a) a.Handle(manager, client);
        }

        public T Read<T>(ByteReadDelegate<T> d)
        {
            var x = d.Invoke(data, out int len, currentOffset);
            currentOffset += len;
            return x;
        }

        public byte ReadNextByte()
        {
            return currentOffset < data.Length ? data[currentOffset++] : default;
        }

        public byte PeekNextByte()
        {
            return currentOffset < data.Length ? data[currentOffset] : default;
        }

        public byte[] ReadRemainingBytes()
        {
            if (RemainingBytes < 1) return new byte[] { };
            return ReadNextBytes(RemainingBytes);
        }

        public byte[] ReadNextBytes(int length)
        {
            byte[] bytes = new byte[length];
            for (int i = 0; i < length && currentOffset < data.Length; i++)
            {
                bytes[i] = data[currentOffset++];
            }
            return bytes;
        }

        public byte[] ReadBytes(int start, int length)
        {
            byte[] newBytes = new byte[length];
            for (int i = 0; i < length && start + i < data.Length; i++)
            {
                newBytes[i] = data[start + i];
            }
            return newBytes;
        }

        public SlotData ReadSlotData()
        {
            throw new NotImplementedException();
        }
    }
}
