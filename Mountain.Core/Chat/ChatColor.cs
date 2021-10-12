using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mountain.Core.Chat
{
    public sealed class ChatColor : ResolvableChatColor
    {
        public static readonly ChatColor Black = new ChatColor('0', 0x00, 0x0, "black", ConsoleColor.Black);
        public static readonly ChatColor DarkBlue = new ChatColor('1', 0x01, 0x0000AA, "dark_blue", ConsoleColor.DarkBlue);
        public static readonly ChatColor DarkGreen = new ChatColor('2', 0x02, 0x00AA00, "dark_green", ConsoleColor.DarkGreen);
        public static readonly ChatColor DarkAqua = new ChatColor('3', 0x03, 0x00AAAA, "dark_aqua", ConsoleColor.DarkCyan);
        public static readonly ChatColor DarkRed = new ChatColor('4', 0x04, 0xAA0000, "dark_red", ConsoleColor.DarkRed);
        public static readonly ChatColor DarkPurple = new ChatColor('5', 0x05, 0xAA00AA, "dark_purple", ConsoleColor.DarkMagenta);
        public static readonly ChatColor Gold = new ChatColor('6', 0x06, 0xFFAA00, "gold", ConsoleColor.DarkYellow);
        public static readonly ChatColor Gray = new ChatColor('7', 0x07, 0xAAAAAA, "gray", ConsoleColor.Gray);
        public static readonly ChatColor DarkGray = new ChatColor('8', 0x08, 0x555555, "dark_gray", ConsoleColor.DarkGray);
        public static readonly ChatColor Blue = new ChatColor('9', 0x09, 0x5555FF, "blue", ConsoleColor.Blue);
        public static readonly ChatColor Green = new ChatColor('a', 0x0A, 0x55FF55, "green", ConsoleColor.Green);
        public static readonly ChatColor Aqua = new ChatColor('b', 0x0B, 0x55FFFF, "aqua", ConsoleColor.Cyan);
        public static readonly ChatColor Red = new ChatColor('c', 0x0C, 0xFF5555, "red", ConsoleColor.Red);
        public static readonly ChatColor LightPurple = new ChatColor('d', 0x0D, 0xFF55FF, "light_purple", ConsoleColor.Magenta);
        public static readonly ChatColor Yellow = new ChatColor('e', 0x0E, 0xFFFF55, "yellow", ConsoleColor.Yellow);
        public static readonly ChatColor White = new ChatColor('f', 0x0F, 0xFFFFFF, "white", ConsoleColor.White);
        public static readonly ChatColor Magic = new ChatColor('k', 0x10);
        public static readonly ChatColor Bold = new ChatColor('l', 0x11);
        public static readonly ChatColor Strikethrough = new ChatColor('m', 0x12);
        public static readonly ChatColor Underline = new ChatColor('n', 0x13);
        public static readonly ChatColor Italic = new ChatColor('o', 0x14);
        public static readonly ChatColor Reset = new ChatColor('r', 0x15, false);

        public const char ColorChar = '\u00A7';
        public const string CodeString = "0123456789AaBbCcDdEeFfKkLlMmNnOoRr";
        public static readonly Regex ColorMatcher = new Regex($"({ColorChar}[0-9A-FK-OR])", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Dictionary<char, ChatColor> CharPairs = new Dictionary<char, ChatColor>();

        static ChatColor()
        {
            CharPairs.Add(Black.CharCode, Black);
            CharPairs.Add(DarkBlue.CharCode, DarkBlue);
            CharPairs.Add(DarkGreen.CharCode, DarkGreen);
            CharPairs.Add(DarkAqua.CharCode, DarkAqua);
            CharPairs.Add(DarkRed.CharCode, DarkRed);
            CharPairs.Add(DarkPurple.CharCode, DarkPurple);
            CharPairs.Add(Gold.CharCode, Gold);
            CharPairs.Add(Gray.CharCode, Gray);
            CharPairs.Add(DarkGray.CharCode, DarkGray);
            CharPairs.Add(Blue.CharCode, Blue);
            CharPairs.Add(Green.CharCode, Green);
            CharPairs.Add(Aqua.CharCode, Aqua);
            CharPairs.Add(Red.CharCode, Red);
            CharPairs.Add(LightPurple.CharCode, LightPurple);
            CharPairs.Add(Yellow.CharCode, Yellow);
            CharPairs.Add(White.CharCode, White);

            CharPairs.Add(Magic.CharCode, Magic);
            CharPairs.Add(Bold.CharCode, Bold);
            CharPairs.Add(Strikethrough.CharCode, Strikethrough);
            CharPairs.Add(Underline.CharCode, Underline);
            CharPairs.Add(Italic.CharCode, Italic);
            CharPairs.Add(Reset.CharCode, Reset);
        }

        public char CharCode { get; }
        public int IntCode { get; }
        public int HexCode { get; }
        public string JsonCode { get; }
        public ConsoleColor ConsoleColor { get; }
        public bool IsFormatter { get; }
        public bool IsColor => !IsFormatter && this != Reset;

        private ChatColor(char code, int intCode, bool formatter = true)
        {
            CharCode = code;
            IntCode = intCode;
            IsFormatter = formatter;
        }

        private ChatColor(char code, int intCode, int hexCode, string jsonCode, ConsoleColor consoleColor)
        {
            CharCode = code;
            IntCode = intCode;
            HexCode = hexCode;
            JsonCode = jsonCode;
            ConsoleColor = consoleColor;
            IsFormatter = false;
        }

        public override string ToJsonValueString() => JsonCode ?? string.Empty;
        public override string ToHexValueString() => IsColor ? '#' + HexCode.ToString("X") : null;
        public override bool IsStandardColor() => IsColor;

        public static ChatColor FromChar(char c)
        {
            if (CharPairs.TryGetValue(c, out var value))
            {
                return value;
            }
            return null;
        }

        public static string StripColor(string input)
        {
            return input == null ? null : ColorMatcher.Replace(input, string.Empty);
        }

        public static string TranslateAlternateColorCodes(char altColorChar, string input)
        {
            char[] chars = input.ToCharArray();

            for (int i = 0; i < chars.Length - 1; i++)
            {
                if (chars[i] == altColorChar && CodeString.IndexOf(chars[i + 1]) > -1)
                {
                    chars[i] = ColorChar;
                    chars[i + 1] = char.ToLower(chars[i + 1]);
                }
            }
            return new string(chars);
        }

        public override string ToString()
        {
            return new string(new char[] { ColorChar, CharCode });
        }
    }
}
