using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core
{
    public interface INamespaceKey
    {
        public Namespace Namespace { get; }
        public string Key { get; }
    }
}
