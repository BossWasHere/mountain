using Mountain.Config.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mountain.Config
{
    public abstract class BaseFileSettings<T>
    {
        protected virtual void Init()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var attrib = info.GetCustomAttributes(typeof(DataField)).FirstOrDefault();
                if (attrib is DataField dataAttrib)
                {
                    if (dataAttrib.HasDefaultValue())
                    {
                        info.SetValue(this, dataAttrib.DefaultValue);
                    }
                }
            }
        }

        protected void Init(Dictionary<string, string> data)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var attrib = info.GetCustomAttributes(typeof(DataField)).FirstOrDefault();
                if (attrib is DataField dataAttrib)
                {
                    string loadedData = data[dataAttrib.Name];
                    bool set = false;
                    if (loadedData != null)
                    {
                        try
                        {
                            set = true;
                            object loadedValue = Convert.ChangeType(loadedData, info.PropertyType);
                            foreach (BaseDataPredicate predicateAttrib in info.GetCustomAttributes(typeof(BaseDataPredicate)))
                            {
                                set = predicateAttrib.Validate(loadedValue);
                                if (!set) break;
                            }
                            if (set) info.SetValue(this, loadedValue);
                        }
                        catch
                        { }
                    }
                    if (!set && dataAttrib.HasDefaultValue())
                    {
                        info.SetValue(this, dataAttrib.DefaultValue);
                    }
                }
            }
        }

        protected Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo info in properties)
            {
                var attrib = info.GetCustomAttributes(typeof(DataField)).FirstOrDefault();
                if (attrib is DataField dataAttrib)
                {
                    object val = info.GetValue(this);
                    if (val == null) val = dataAttrib.DefaultValue;
                    if (val == null) val = string.Empty;
                    data.Add(dataAttrib.Name, val.ToString());
                }
            }

            return data;
        }

        protected abstract void Write(Dictionary<string, string> data, string filePath);
    }
}
