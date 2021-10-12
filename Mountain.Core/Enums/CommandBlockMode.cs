using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Enums
{
    public enum CommandBlockMode
    {
        Sequence = 0, // Chain
        Auto = 1, // Repeating
        Redstone = 2 // Impulse
    }
}
