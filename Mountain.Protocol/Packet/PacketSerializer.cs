using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.Packet
{

    public class PacketSerializer
    {
        public MemoryStream Stream { get; }

        public byte PacketId { get; }

        public PacketSerializer(byte packetId)
        {
            PacketId = packetId;
            Stream = new MemoryStream();
            Stream.WriteByte(packetId);
        }

        public void WriteByte(byte b)
        {
            Stream.WriteByte(b);
        }

        public void WriteBytes(byte[] bytes)
        {
            Stream.Write(bytes);
        }

        public void WriteBytes(byte[] bytes, int length, int offset = 0)
        {
            Stream.Write(bytes, offset, length);
        }

        public MemoryStream ToDataStream(bool compress = false, int compressionThreshold = 256)
        {
            var length = (int)Stream.Length;
            var modifiedStream = new MemoryStream();

            //DataTypes.WriteVarInt(modifiedStream, length);
            //Stream.CopyTo(modifiedStream);
            //length = (int)modifiedStream.Length;

            if (compress)
            {
                if (length < compressionThreshold)
                {
                    modifiedStream.WriteVarInt(length + 1);
                    modifiedStream.WriteByte(0);

                    Stream.Position = 0;
                    Stream.CopyTo(modifiedStream);
                    return modifiedStream;
                }

                var dos = new DeflaterOutputStream(Stream);
                if (dos.Length > int.MaxValue) throw new InvalidDataException("Too many bytes");
                int compressedLength = (int)dos.Length;

                byte[] lenBytes = DataTypes.WriteVarInt(compressedLength);
                DataTypes.WriteVarInt(modifiedStream, lenBytes.Length + compressedLength);
                modifiedStream.Write(lenBytes);
                dos.CopyTo(modifiedStream);

                return modifiedStream;
            }

            DataTypes.WriteVarInt(modifiedStream, length);
            Stream.Position = 0;
            Stream.CopyTo(modifiedStream);

            return modifiedStream;
        }

    }
}
