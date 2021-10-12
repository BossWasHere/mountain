using Mountain.Protocol.NBT.Data;
using System;

namespace Mountain.Protocol.NBT
{
    public static class NBTTags
    {
        public static INBTTagType GetTagType(int tagId)
        {
            return tagId >= 0 && tagId < tagTypes.Length ? tagTypes[tagId] : null;
        }

        public static readonly NBTTagType<NBTTagEnd> TagEnd = new NBTTagType<NBTTagEnd>(0x0, 0);
        public static readonly NBTTagType<NBTTagByte> TagByte = new NBTTagType<NBTTagByte>(0x01, 1);
        public static readonly NBTTagType<NBTTagShort> TagShort = new NBTTagType<NBTTagShort>(0x02, 2);
        public static readonly NBTTagType<NBTTagInt> TagInt = new NBTTagType<NBTTagInt>(0x03, 4);
        public static readonly NBTTagType<NBTTagLong> TagLong = new NBTTagType<NBTTagLong>(0x04, 8);
        public static readonly NBTTagType<NBTTagFloat> TagFloat = new NBTTagType<NBTTagFloat>(0x05, 4);
        public static readonly NBTTagType<NBTTagDouble> TagDouble = new NBTTagType<NBTTagDouble>(0x06, 8);
        public static readonly NBTTagType<NBTTagByteArray> TagByteArray = new NBTTagType<NBTTagByteArray>(0x07);
        public static readonly NBTTagType<NBTTagString> TagString = new NBTTagType<NBTTagString>(0x08);
        public static readonly NBTTagType<NBTTagList> TagList = new NBTTagType<NBTTagList>(0x09);
        public static readonly NBTTagType<NBTTagCompound> TagCompound = new NBTTagType<NBTTagCompound>(0x0A);
        public static readonly NBTTagType<NBTTagIntArray> TagIntArray = new NBTTagType<NBTTagIntArray>(0x0B);
        public static readonly NBTTagType<NBTTagLongArray> TagLongArray = new NBTTagType<NBTTagLongArray>(0x0C);

        private static readonly INBTTagType[] tagTypes = new INBTTagType[]
        {
            TagEnd, TagByte, TagShort, TagInt,
            TagLong, TagFloat, TagDouble, TagByteArray,
            TagString, TagList, TagCompound, TagIntArray, TagLongArray
        };
    }

    public interface INBTTagType
    {
        public byte TagId { get; }
        public int PayloadSize { get; }
        public NBTTag Create();
        public Type GetNBTTagType();
    }

    public class NBTTagType<T> : INBTTagType where T : NBTTag, new()
    {
        public byte TagId { get; }
        public int PayloadSize { get; }

        public NBTTagType(byte tagId, int payloadSize = -1)
        {
            TagId = tagId;
            PayloadSize = payloadSize;
        }

        public T Create() => new T();

        NBTTag INBTTagType.Create() => Create();

        public Type GetNBTTagType()
        {
            return typeof(T);
        }
    }
}
