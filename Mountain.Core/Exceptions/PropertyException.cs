using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Exceptions
{
    public class PropertyException : Exception
    {
        public PropertyException(string message) : base(message)
        { }
    }
}
