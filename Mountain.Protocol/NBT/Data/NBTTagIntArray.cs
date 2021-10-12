using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagIntArray : TypeNBTTag<int[]>
    {
        public override byte TagId => NBTTags.TagIntArray.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            int intLength = Value?.Length ?? 0;
            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), (intLength * 4) + 5, 1);
            bytes[0] = TagId;

            byte[] lengthBytes = DataTypes.WriteInt(intLength);

            bytes[1] = lengthBytes[0];
            bytes[2] = lengthBytes[1];
            bytes[3] = lengthBytes[2];
            bytes[4] = lengthBytes[3];

            if (Value == null) return bytes;

            intLength = 5;
            for (int i = 0; i < Value.Length; i++)
            {
                byte[] v = DataTypes.WriteInt(Value[i]);
                bytes[intLength++] = v[0];
                bytes[intLength++] = v[1];
                bytes[intLength++] = v[2];
                bytes[intLength++] = v[3];
            }
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            int intLength = DataTypes.ReadInt(bytes, out _, offset += length);
            if (intLength < 0) throw new NBTException("Invalid length parsed for NBT Int Array");

            Value = new int[intLength];
            for (int i = 0; i < intLength; i++)
            {
                offset += 4;
                Value[i] = DataTypes.ReadInt(bytes, out _, offset);
            }
            length += 4 + (intLength * 4);
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteIntPrefixedIntArray(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadIntPrefixedIntArray();
        }
    }
}
