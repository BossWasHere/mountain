using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Mountain.Core;
using System;
using System.IO;

/*
 * Notes:
 * Inflater and Deflater streams start inflating/deflating at the current position of the underlying stream
 */


namespace Mountain.Protocol.Packet
{
    public static class PacketUtils
    {

        /// <summary>
        /// Reads and assembles incoming/serverbound packet data from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="compressed">If the packet is in the compressed format</param>
        /// <param name="state">The current state of the connection that the packet was received over</param>
        /// <returns>The packet data, or error data on failure</returns>
        public static PacketReadData ReadPacket(this Stream stream, bool compressed, ConnectionState state)
        {
            return stream.ReadPacketInternal(compressed, false, state);
        }

        /// <summary>
        /// Reads and assembles outgoing/clientbound packet data from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="compressed">If the packet is in the compressed format</param>
        /// <param name="state">The current state of the connection that the packet was sent over</param>
        /// <returns>The packet data, or error data on failure</returns>
        public static PacketReadData ReadClientboundPacket(this Stream stream, bool compressed, ConnectionState state)
        {
            return stream.ReadPacketInternal(compressed, true, state);
        }

        //TODO check the impact of not closing streams on this method (ONLY the decompression stream)
        private static PacketReadData ReadPacketInternal(this Stream stream, bool compressed, bool clientbound, ConnectionState state)
        {
            if (!stream.CanRead) throw new ArgumentException("Cannot read packet from non-readable stream");
            var rawLength = stream.ReadVarInt();
            bool closeStream = false;
            Stream readerStream;
            // Assume uncompressed: Data Length = Raw Length - Length(PacketId)
            var dataLength = rawLength - 1;

            if (compressed)
            {
                var uncompressedLength = stream.ReadVarInt(out int compressedLength);
                if (uncompressedLength == 0)
                {
                    readerStream = stream;
                }
                else
                {
                    compressedLength = rawLength - compressedLength;

                    readerStream = new MemoryStream(uncompressedLength);

                    // Always copy stream - if we close the inflater stream, the base stream is closed too
                    using var copyStream = new MemoryStream();
                    stream.CopyRangeTo(copyStream, compressedLength);
                    copyStream.Seek(0, SeekOrigin.Begin);

                    using var iis = new InflaterInputStream(copyStream);
                    iis.CopyTo(readerStream);

                    readerStream.Seek(0, SeekOrigin.Begin);

                    // If compressed: Data Length = Decompressed Length - Length(PacketId)
                    dataLength = (int)readerStream.Length - 1;
                    closeStream = true;
                }
            }
            else
            {
                readerStream = stream;
            }

            var packetId = readerStream.ReadByte();
            if (packetId == -1)
            {
                return new PacketReadData(null, DeserializeState.TooShort, compressed, packetId);
            }

            IPacketDeserializable packet;
            if (clientbound)
            {
                var packetType = Packets.GetOutboundType((byte)packetId, state);
                if (packetType == null)
                {
                    return new PacketReadData(null, DeserializeState.UnknownPacket, compressed, packetId);
                }
                var basePacket = packetType.GetBase();
                if (basePacket is IPacketDeserializable pd)
                {
                    packet = pd;
                }
                else
                {
                    return new PacketReadData(null, DeserializeState.UnknownPacket, compressed, packetId);
                }
            }
            else
            {
                var packetType = Packets.GetInboundType((byte)packetId, state);
                if (packetType == null)
                {
                    return new PacketReadData(null, DeserializeState.UnknownPacket, compressed, packetId);
                }
                packet = packetType.GetBase();
            }
            
            try
            {
                packet.ReadFromStream(readerStream, dataLength);
            }
            catch
            {
                return new PacketReadData(packet, DeserializeState.BadData, compressed, packetId);
            }
            if (closeStream) try { readerStream.Close(); } catch { }

            return new PacketReadData(packet, DeserializeState.Done, compressed, packetId);
        }

        /// <summary>
        /// Writes packet data to the provided stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="packet">The packet data to write</param>
        /// <param name="compressionThreshold">The size threshold before data is compressed, or -1 to disable compression</param>
        public static void WritePacket(this Stream stream, IOutboundPacket packet, int compressionThreshold)
        {
            if (packet == null) throw new ArgumentNullException(nameof(packet));
            if (!stream.CanWrite) throw new ArgumentException("Cannot write packet to non-writable stream");

            using var packetStream = new MemoryStream();
            packet.WriteToStream(packetStream);
            packetStream.Position = 0;

            var length = (int)packetStream.Length;
            if (compressionThreshold <= -1)
            {
                stream.WriteVarInt(length);
                packetStream.CopyTo(stream);
            }
            else
            {
                if (length < compressionThreshold)
                {
                    stream.WriteVarInt(length + 1);
                    stream.WriteByte(0);
                    packetStream.CopyTo(stream);
                }
                else
                {
                    using var compressStream = new DeflaterOutputStream(packetStream);
                    using var copyStream = new MemoryStream();
                    compressStream.CopyTo(copyStream);
                    var compressedLength = (int)copyStream.Length;
                    byte[] uncompressedLengthBytes = DataTypes.WriteVarInt(length);

                    stream.WriteVarInt(compressedLength + uncompressedLengthBytes.Length);
                    stream.Write(uncompressedLengthBytes);
                    copyStream.CopyTo(stream);
                }
            }
        }

        /// <summary>
        /// Reads a number of bytes from the current stream and writes them to another stream
        /// </summary>
        /// <param name="stream">The source stream to read from</param>
        /// <param name="destination">The destination stream to write to</param>
        /// <param name="bytes">The number of bytes to transfer</param>
        public static void CopyRangeTo(this Stream stream, Stream destination, int bytes)
        {
            int bSize = bytes - (bytes % 16) + 16;
            byte[] buffer = new byte[Math.Min(32768, bSize)];
            int read;
            while (bytes > 0 && (read = stream.Read(buffer, 0, Math.Min(buffer.Length, bytes))) > 0)
            {
                destination.Write(buffer, 0, read);
                bytes -= read;
            }
        }
    }
}
