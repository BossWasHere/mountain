using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagLong : TypeNBTTag<long>
    {
        public override byte TagId => NBTTags.TagLong.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            Validate();

            byte[] bytes = DataTypes.CopyWithExtraCapacity(DataTypes.WriteShortString(Name), 9, 1);
            bytes[0] = TagId;

            byte[] longBytes = DataTypes.WriteLong(Value);
            int i = bytes.Length - 8;
            bytes[i++] = longBytes[0];
            bytes[i++] = longBytes[1];
            bytes[i++] = longBytes[2];
            bytes[i++] = longBytes[3];
            bytes[i++] = longBytes[4];
            bytes[i++] = longBytes[5];
            bytes[i++] = longBytes[6];
            bytes[i] = longBytes[7];
            return bytes;
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            Name = DataTypes.ReadShortString(bytes, out length, offset);
            Value = DataTypes.ReadLong(bytes, out _, offset + length);
            length += 8;
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteLong(Value);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            Value = stream.ReadLong();
        }
    }
}
