using Mountain.Core;
using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagList : TypeNBTTag<INBTList>
    {
        public override byte TagId => NBTTags.TagList.TagId;

        [Obsolete]
        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public override void SetBytes(byte[] bytes, out int length, int offset = 0)
        {
            throw new NotImplementedException();
        }

        public override void WriteToStream(Stream stream, bool listItem = false)
        {
            base.WriteToStream(stream, listItem);
            stream.WriteByte(Value.GetNBTListTypeIndex());
            stream.WriteInt(Value.Count);

            foreach (NBTTag tag in Value)
            {
                tag.WriteToStream(stream, true);
            }
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);
            INBTTagType type = NBTTags.GetTagType(stream.ReadByte());
            int count = stream.ReadInt();

            var list = new DynamicNBTList(type);
            for (int i = 0; i < count; i++)
            {
                var nbtItem = type.Create();
                nbtItem.ReadFromStream(stream, true);
                list.Add(nbtItem);
            }
               
            Value = list;
        }
    }
}
