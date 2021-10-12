using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagEnd : NBTTag
    {
        public override byte TagId => NBTTags.TagEnd.TagId;

        [Obsolete]
        public override byte[] GetBytes() => new byte[0];

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset) => length = 0;

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, true);
            stream.WriteByte(TagId);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        { }

        public static void WriteToStreamStatic(System.IO.Stream stream)
        {
            stream.WriteByte(NBTTags.TagEnd.TagId);
        }
    }
}
