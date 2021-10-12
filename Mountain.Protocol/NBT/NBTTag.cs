using Mountain.Core;
using System;

namespace Mountain.Protocol.NBT
{
    public abstract class NBTTag
    {
        public abstract byte TagId { get; }

        private protected string Name;

        public string GetName()
        {
            return Name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void Validate(bool asListItem = false)
        {
            if (!asListItem && string.IsNullOrEmpty(GetName())) throw new NullReferenceException("NBT tag name cannot be null");
        }

        public virtual void ReadFromStream(System.IO.Stream stream, bool listItem = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new NBTException("The stream is not readable");

            if (!listItem)
            {
                Name = stream.ReadShortString();
            }
        }

        public virtual void WriteToStream(System.IO.Stream stream, bool listItem = false)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite) throw new NBTException("The stream is not writable");

            Validate(listItem);
            if (!listItem)
            {
                stream.WriteByte(TagId);
                stream.WriteShortString(GetName());
            }
        }

        [Obsolete("Use stream functions instead")]
        public abstract byte[] GetBytes();
        [Obsolete("Use stream functions instead")]
        public abstract void SetBytes(byte[] bytes, out int length, int offset = 0);
    }

    public abstract class TypeNBTTag<T> : NBTTag
    {
        private protected T Value;

        public T GetValue()
        {
            return Value;
        }

        public void SetValue(T value)
        {
            Value = value;
        }
    }
}
