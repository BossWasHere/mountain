using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Exceptions
{
    public class DataReadException : Exception
    {
        public DataReadException(string message) : base(message)
        {
        }
    }
}
