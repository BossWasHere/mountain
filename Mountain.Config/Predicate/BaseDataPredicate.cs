using System;

namespace Mountain.Config.Predicate
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class BaseDataPredicate : Attribute
    {
        public abstract bool Validate(object obj);
    }
}
