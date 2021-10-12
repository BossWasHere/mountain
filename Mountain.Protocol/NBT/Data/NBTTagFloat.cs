using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagFloat : TypeNBTTag<float>
    {
        public override byte TagId => NBTTags.TagFloat.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), 5, 1);
            bytes[0] = TagId;

            byte[] intBytes = DataTypes.WriteFloat(Value);
            int i = bytes.Length - 4;
            bytes[i++] = intBytes[0];
            bytes[i++] = intBytes[1];
            bytes[i++] = intBytes[2];
            bytes[i] = intBytes[3];
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            Value = DataTypes.ReadFloat(bytes, out _, offset + length);
            length += 4;
        }
        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteFloat(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadFloat();
        }
    }
}
