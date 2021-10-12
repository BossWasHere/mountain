using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagDouble : TypeNBTTag<double>
    {
        public override byte TagId => NBTTags.TagDouble.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), 9, 1);
            bytes[0] = TagId;
            byte[] intBytes = DataTypes.WriteDouble(Value);
            int i = bytes.Length - 8;
            bytes[i++] = intBytes[0];
            bytes[i++] = intBytes[1];
            bytes[i++] = intBytes[2];
            bytes[i++] = intBytes[3];
            bytes[i++] = intBytes[4];
            bytes[i++] = intBytes[5];
            bytes[i++] = intBytes[6];
            bytes[i] = intBytes[7];
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            Value = DataTypes.ReadDouble(bytes, out _, offset + length);
            length += 8;
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteDouble(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadDouble();
        }
    }
}
