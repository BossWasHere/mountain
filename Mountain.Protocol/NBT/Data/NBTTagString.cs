using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagString : TypeNBTTag<string>
    {
        public override byte TagId => NBTTags.TagString.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            byte[] valueBytes = DataTypes.WriteShortString(Value ?? string.Empty);
            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), 1 + valueBytes.Length, 1);

            bytes[0] = TagId;

            int i = bytes.Length - valueBytes.Length;
            for (int j = 0; j < valueBytes.Length; i++, j++)
            {
                bytes[i] = valueBytes[j];
            }
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            Value = DataTypes.ReadShortString(bytes, out int extLength, offset + length);
            length += extLength;
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteShortString(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadShortString();
        }
    }
}
