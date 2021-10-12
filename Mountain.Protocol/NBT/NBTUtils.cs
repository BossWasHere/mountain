using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Protocol.NBT
{
    public static class NBTUtils
    {

        public static NBTTag ReadNextTag(this Stream stream)
        {
            var tagType = NBTTags.GetTagType(stream.ReadByte());
            if (tagType == null) return null;

            var tag = tagType.Create();

            tag.ReadFromStream(stream);
            return tag;
        }

        public static NBTRoot ReadNBT(byte[] bytes)
        {
            var stream = new NBTCompressionStream(bytes);
            NBTTag next = stream.ReadNextTag();
            NBTRoot root = new NBTRoot();
            while (next != null)
            {
                root.Add(next.GetName(), next);
                next = stream.ReadNextTag();
            }
            return root;
        }

        public static NBTRoot ReadNBT(string path)
        {
            return ReadNBT(File.ReadAllBytes(path));
        }
    }
}
