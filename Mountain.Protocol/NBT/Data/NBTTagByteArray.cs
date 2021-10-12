using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagByteArray : TypeNBTTag<byte[]>
    {
        public override byte TagId => NBTTags.TagByteArray.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            int byteLength = Value?.Length ?? 0;
            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), byteLength + 5, 1);
            bytes[0] = TagId;

            byte[] lengthBytes = DataTypes.WriteInt(byteLength);

            bytes[1] = lengthBytes[0];
            bytes[2] = lengthBytes[1];
            bytes[3] = lengthBytes[2];
            bytes[4] = lengthBytes[3];

            if (Value == null) return bytes;

            byteLength = 5;
            for (int i = 0; i < Value.Length; i++)
            {
                bytes[byteLength++] = Value[i];
            }
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            int byteLength = DataTypes.ReadInt(bytes, out _, offset += length);
            if (byteLength < 0) throw new NBTException("Invalid length parsed for NBT Byte Array");
            offset += 4;

            Value = new byte[byteLength];
            for (int i = 0; i < byteLength; i++)
            {
                Value[i] = bytes[offset++];
            }
            length += 4 + byteLength;
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteIntPrefixedByteArray(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadIntPrefixedByteArray();
        }
    }
}
