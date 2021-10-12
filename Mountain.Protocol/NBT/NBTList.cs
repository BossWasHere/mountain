using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mountain.Protocol.NBT
{
    public interface INBTList : IEnumerable
    {
        public string Name { get; set; }
        public int Count { get; }

        public Type GetNBTListType();
        public Type GetListType();
        public byte GetNBTListTypeIndex();
        public IList<T> CastTo<T>() where T : NBTTag;
    }

    public class DynamicNBTList : IList<NBTTag>, INBTList
    {

        private readonly List<NBTTag> list;
        public INBTTagType Type { get; private set; }
        public bool CanChangeType { get; }

        public DynamicNBTList()
        {
            list = new List<NBTTag>();
            CanChangeType = true;
        }

        public DynamicNBTList(int capacity)
        {
            list = new List<NBTTag>(capacity);
            CanChangeType = true;
        }

        public DynamicNBTList(INBTTagType type)
        {
            list = new List<NBTTag>();
            CanChangeType = false;
            Type = type;
        }

        public string Name { get; set; }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public NBTTag this[int index] { get => list[index]; set => throw new NotSupportedException("NBTList order is not guaranteed, use Add()"); }

        public Type GetNBTListType()
        {
            return CanChangeType ? Count > 0 ? this[0].GetType() : null : Type.GetNBTTagType();
        }

        public Type GetListType()
        {
            var type = GetNBTListType()?.BaseType;
            if (type == null) return null;
            return type.IsGenericType ? type.GetGenericArguments()[0] : null;
        }

        public byte GetNBTListTypeIndex()
        {
            return CanChangeType ? Count > 0 ? this[0].TagId : (byte)0xFF : Type.TagId;
        }

        public IList<C> CastTo<C>() where C : NBTTag
        {
            Type to = typeof(C);
            if (to.Equals(GetNBTListType()))
            {
                return new List<C>(this.Cast<C>());
            }
            throw new InvalidCastException("Cannot cast to " + to.Name);
        }

        public void Add(NBTTag item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (CanChangeType ? Count > 0 && !GetNBTListType().Equals(item.GetType()) : item.TagId != Type.TagId) throw new NBTException("NBTList only supports one NBT tag type");
            list.Add(item);
        }

        public int IndexOf(NBTTag item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, NBTTag item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (CanChangeType ? Count > 0 && !GetNBTListType().Equals(item.GetType()) : item.TagId != Type.TagId) throw new NBTException("NBTList only supports one NBT tag type");
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(NBTTag item)
        {
            return list.Contains(item);
        }

        public void CopyTo(NBTTag[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(NBTTag item)
        {
            return list.Remove(item);
        }

        public IEnumerator<NBTTag> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    //public class NBTList<T, U> : List<U>, INBTList where T : TypeNBTTag<U>, new()
    public class NBTList<T> : List<T>, INBTList where T : NBTTag, new()
    {
        public string Name { get; set; }
        private readonly T tag;

        public NBTList()
        {
            tag = new T();
        }

        public byte GetNBTListTypeIndex()
        {
            return tag.TagId;
        }

        public Type GetNBTListType()
        {
            return typeof(T);
        }

        public Type GetListType()
        {
            var type = GetNBTListType()?.BaseType;
            if (type == null) return null;
            return type.IsGenericType ? type.GetGenericArguments()[0] : null;
        }

        public IList<C> CastTo<C>() where C : NBTTag
        {
            var to = typeof(C);
            if (to.IsAssignableFrom(GetNBTListType()))
            {
                return new List<C>(this.Cast<C>());
            }
            throw new InvalidCastException("Cannot cast to " + to.Name);
        }
    }
}
