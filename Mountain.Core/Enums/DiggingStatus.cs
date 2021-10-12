using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Enums
{
    public enum DiggingStatus
    {
        Started = 0,
        Cancelled = 1,
        Finished = 2,
        DropItemStack = 3,
        DropItem = 4,
        ArrowOrEatingFinished = 5,
        SwapItems = 6
    }
}
