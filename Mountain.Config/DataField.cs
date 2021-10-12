using System;

namespace Mountain.Config
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataField : Attribute
    {
        public string Name { get; }
        public object DefaultValue { get; }
        public DataField(string name)
        {
            Name = name;
        }
        public DataField(string name, bool defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, string defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, int defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, uint defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, long defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, ulong defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, short defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, ushort defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }
        public DataField(string name, byte defaultValue) : this(name)
        {
            DefaultValue = defaultValue;
        }

        public bool HasDefaultValue() => DefaultValue != null;
    }
}
