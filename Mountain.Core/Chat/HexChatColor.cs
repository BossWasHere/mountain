using System;

namespace Mountain.Core.Chat
{
    public sealed class HexChatColor : ResolvableChatColor
    {
        public const int Max = 16777215;
        private const int bitMask = 0xFF;
        public int HexCode
        {
            get
            {
                return (bitMask & Red) << 16 | (bitMask & Green) << 8 | (bitMask & Blue);
            }
            set
            {
                if (value > Max || value < 0) throw new ArgumentOutOfRangeException(nameof(HexCode), "Hex color codes must be less than 2^24");
                Red = value >> 16;
                Green = value >> 8 & bitMask;
                Blue = value & bitMask;
            }
        }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public HexChatColor()
        { }

        public HexChatColor(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public HexChatColor(int hexCode)
        {
            HexCode = hexCode;
        }

        public override string ToJsonValueString() => ToHexValueString();

        public override string ToHexValueString() => '#' + HexCode.ToString("X");

        public override bool IsStandardColor() => false;

        public override string ToString() => ToHexValueString();
    }
}
