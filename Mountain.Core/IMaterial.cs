using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core
{
    public interface IMaterial : INamespaceKey
    {
        public string Name { get; }
        public bool IsBlock { get; }
        public bool IsItem { get; }
        public bool IsSolid { get; }
        public bool IsEdible { get; }
        public bool IsRecord { get; }
        public bool IsAir { get; }
    }
}
