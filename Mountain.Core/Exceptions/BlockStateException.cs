using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Exceptions
{
    public class BlockStateException : Exception
    {
        public BlockStateException(string message) : base(message)
        { }
    }
}
