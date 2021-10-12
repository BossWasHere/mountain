using Mountain.Protocol.NBT.Data;
using System.Collections.Generic;
using System.Linq;

namespace Mountain.Protocol.NBT
{
    public class NBTCompound : Dictionary<string, NBTTag>
    {
        public virtual bool IsRoot => false;
        public string Name { get; set; }

        public NBTTagCompound ToTagCompound()
        {
            var c = new NBTTagCompound();
            if (!string.IsNullOrEmpty(Name)) c.SetName(Name);
            c.SetValue(this);

            return c;
        }

        public NBTCompound ExpandRoot()
        {
            if (Count == 1)
            {
                if (this.First().Value is NBTTagCompound ntc)
                {
                    return ntc.GetValue()?.ExpandRoot() ?? this;
                }
            }
            return this;
        }

        public T Get<T>(string key) where T : NBTTag
        {
            return this[key] is T t ? t : null;
        }

        public byte GetByte(string key)
        {
            return this[key] is NBTTagByte t ? t.GetValue() : throw new NBTException("Value is not a byte: " + key);
        }

        public byte[] GetByteArray(string key)
        {
            return this[key] is NBTTagByteArray t ? t.GetValue() : null;
        }

        public NBTCompound GetCompound(string key)
        {
            return this[key] is NBTTagCompound t ? t.GetValue() : null;
        }

        public double GetDouble(string key)
        {
            return this[key] is NBTTagDouble t ? t.GetValue() : throw new NBTException("Value is not a double: " + key);
        }

        public float GetFloat(string key)
        {
            return this[key] is NBTTagFloat t ? t.GetValue() : throw new NBTException("Value is not a float: " + key);
        }

        public int GetInt(string key)
        {
            return this[key] is NBTTagInt t ? t.GetValue() : throw new NBTException("Value is not an int: " + key);
        }

        public int[] GetIntArray(string key)
        {
            return this[key] is NBTTagIntArray t ? t.GetValue() : null;
        }

        public INBTList GetList(string key)
        {
            return this[key] is NBTTagList t ? t.GetValue() : null;
        }

        public long GetLong(string key)
        {
            return this[key] is NBTTagLong t ? t.GetValue() : throw new NBTException("Value is not a long: " + key);
        }

        public long[] GetLongArray(string key)
        {
            return this[key] is NBTTagLongArray t ? t.GetValue() : null;
        }

        public short GetShort(string key)
        {
            return this[key] is NBTTagShort t ? t.GetValue() : throw new NBTException("Value is not a short: " + key);
        }

        public string GetString(string key)
        {
            return this[key] is NBTTagString t ? t.GetValue() : null;
        }
    }

    public class NBTRoot : NBTCompound
    {
        public override bool IsRoot => true;
    }
}
