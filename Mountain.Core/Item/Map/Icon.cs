using Mountain.Core.Chat;
using Mountain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mountain.Core.Item.Map
{
    public struct Icon
    {
        public MapIconType Type {  get; set; }
        public byte X { get; set; }
        public byte Z {  get; set; }
        public byte Direction { get; set; }
        public ChatMessage DisplayName { get; set; }
    }
}
