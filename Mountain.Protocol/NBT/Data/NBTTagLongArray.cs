using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagLongArray : TypeNBTTag<long[]>
    {
        public override byte TagId => NBTTags.TagLongArray.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            int longLength = Value?.Length ?? 0;
            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), (longLength * 8) + 5, 1);
            bytes[0] = TagId;

            byte[] lengthBytes = DataTypes.WriteInt(longLength);

            bytes[1] = lengthBytes[0];
            bytes[2] = lengthBytes[1];
            bytes[3] = lengthBytes[2];
            bytes[4] = lengthBytes[3];

            if (Value == null) return bytes;

            longLength = 5;
            for (int i = 0; i < Value.Length; i++)
            {
                byte[] v = DataTypes.WriteLong(Value[i]);
                bytes[longLength++] = v[0];
                bytes[longLength++] = v[1];
                bytes[longLength++] = v[2];
                bytes[longLength++] = v[3];
                bytes[longLength++] = v[4];
                bytes[longLength++] = v[5];
                bytes[longLength++] = v[6];
                bytes[longLength++] = v[7];
            }
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            int longLength = DataTypes.ReadInt(bytes, out _, offset += length);
            if (longLength < 0) throw new NBTException("Invalid length parsed for NBT Long Array");
            offset += 4;

            Value = new long[longLength];
            for (int i = 0; i < longLength; i++)
            {
                Value[i] = DataTypes.ReadLong(bytes, out _, offset);
                offset += 8;
            }
            length += 4 + (longLength * 8);
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteIntPrefixedLongArray(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadIntPrefixedLongArray();
        }
    }
}
