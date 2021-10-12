using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Protocol.NBT
{
    public class NBTException : Exception
    {
        public NBTException()
        { }

        public NBTException(string message) : base(message)
        { }
    }
}
