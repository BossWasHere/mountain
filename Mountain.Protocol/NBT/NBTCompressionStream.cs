using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

namespace Mountain.Protocol.NBT
{
    public class NBTCompressionStream : Stream
    {
        private readonly Stream underlyingStream;
        private readonly bool NBTLoaded;
        private bool isClosed = false;

        public NBTCompression CompressionMode { get; }

        public NBTCompressionStream(NBTCompression compressionMode = NBTCompression.GZip)
        {
            underlyingStream = GetCompressionStream(compressionMode);
            CompressionMode = compressionMode;
            NBTLoaded = false;
        }

        public NBTCompressionStream(byte[] bytes)
        {
            underlyingStream = GetDecompressionStream(bytes, out NBTCompression mode);
            CompressionMode = mode;
            NBTLoaded = true;
        }

        public NBTCompressionStream(byte[] bytes, int offset, int length)
        {
            underlyingStream = GetDecompressionStream(bytes, offset, length, out NBTCompression mode);
            CompressionMode = mode;
            NBTLoaded = true;
        }

        public override bool CanRead => underlyingStream.CanRead;

        public override bool CanSeek => underlyingStream.CanSeek;

        public override bool CanWrite => underlyingStream.CanWrite && !NBTLoaded;

        public override long Length => underlyingStream.Length;

        public override long Position { get => underlyingStream.Position; set => underlyingStream.Position = value; }

        public override void Flush()
        {
            underlyingStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return underlyingStream.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            return underlyingStream.Read(buffer);
        }

        public override int ReadByte()
        {
            return underlyingStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlyingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("Cannot set length of NBT Compression Stream");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite) throw new NotSupportedException("Cannot write to loaded NBT Compression Stream");
            underlyingStream.Write(buffer, offset, count);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (!CanWrite) throw new NotSupportedException("Cannot write to loaded NBT Compression Stream");
            underlyingStream.Write(buffer);
        }

        public override void Close()
        {
            if (!isClosed)
            {
                isClosed = true;
                underlyingStream.Close();
            }
        }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            underlyingStream.CopyTo(destination, bufferSize);
        }

        public static Stream GetDecompressionStream(byte[] bytes, out NBTCompression mode)
        {
            var stream = new MemoryStream(bytes);
            return CreateDecompressionStream(stream, bytes[0], out mode);
        }

        public static Stream GetDecompressionStream(Stream stream, out NBTCompression mode)
        {
            var b = stream.ReadByte();
            stream.Position--;
            return CreateDecompressionStream(stream, b, out mode);
        }

        public static Stream GetDecompressionStream(byte[] bytes, int offset, int length, out NBTCompression mode)
        {
            var stream = new MemoryStream(bytes, offset, length);
            return CreateDecompressionStream(stream, bytes[offset], out mode);
        }

        public static Stream GetDecompressionStream(Stream stream, int length, out NBTCompression mode)
        {
            var newStream = new MemoryStream();
            stream.CopyTo(newStream, length);
            var b = newStream.ReadByte();
            newStream.Position--;
            return CreateDecompressionStream(newStream, b, out mode);
        }

        private static Stream CreateDecompressionStream(Stream stream, int b, out NBTCompression mode)
        {
            switch (b)
            {
                case 0x0A:
                    mode = NBTCompression.None;
                    return stream;
                case 0x1F:
                    mode = NBTCompression.GZip;
                    return new GZipInputStream(stream);
                case 0x78:
                    mode = NBTCompression.ZLib;
                    return new InflaterInputStream(stream);
                default:
                    throw new NBTException("Could not auto-detect compression format.");
            }
        }

        public static Stream GetCompressionStream(NBTCompression mode)
        {
            return mode switch
            {
                NBTCompression.None => new MemoryStream(),
                NBTCompression.GZip => new GZipOutputStream(new MemoryStream()),
                NBTCompression.ZLib => new DeflaterOutputStream(new MemoryStream()),
                _ => throw new NBTException("An error occured - unknown compression type"),
            };
        }
    }
}
