using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core
{
    public struct SlotData
    {
        public bool IsPresent { get; set; }
        public int ItemId { get; set; }
        public byte ItemCount { get; set; }
        public string NBTData { get; set; }
    }
}
