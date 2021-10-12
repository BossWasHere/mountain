using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagShort : TypeNBTTag<short>
    {
        public override byte TagId => NBTTags.TagShort.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), 3, 1);
            bytes[0] = TagId;

            byte[] shortBytes = DataTypes.WriteShort(Value);
            bytes[^2] = shortBytes[0];
            bytes[^1] = shortBytes[1];
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            Value = DataTypes.ReadShort(bytes, out _, offset + length);
            length += 2;
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteShort(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadShort();
        }
    }
}
