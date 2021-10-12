using System;
using System.IO;

namespace Mountain.Protocol.NBT.Data
{
    public sealed class NBTTagCompound : TypeNBTTag<NBTCompound>
    {
        public override byte TagId => NBTTags.TagCompound.TagId;

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

            if (Value != null)
            {
                foreach (NBTTag tag in Value.Values)
                {
                    tag.WriteToStream(stream);
                }
            }

            NBTTagEnd.WriteToStreamStatic(stream);
        }

        public override void ReadFromStream(Stream stream, bool listItem = false)
        {
            base.ReadFromStream(stream, listItem);

            NBTTag tag = stream.ReadNextTag();
            Value = new NBTCompound();

            while (!(tag is NBTTagEnd))
            {
                Value.Add(tag.GetName(), tag);
                tag = stream.ReadNextTag();
            }
        }
    }
}
