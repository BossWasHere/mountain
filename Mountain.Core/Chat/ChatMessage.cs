using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mountain.Core.Chat
{
    public class ChatMessage : BaseChatMessage
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        public override byte[] SerializeToUtf8Bytes()
        {
            return SerializeToUtf8Bytes(typeof(ChatMessage));
        }

        public override string Serialize()
        {
            return Serialize(typeof(ChatMessage));
        }

        public static ChatMessage Deserialize(byte[] raw)
        {
            return JsonSerializer.Deserialize<ChatMessage>(raw);
        }

        public static ChatMessage DeserializeSafe(byte[] raw)
        {
            try
            {
                return Deserialize(raw);
            }
            catch
            {
                return default;
            }
        }

        public static ChatMessage[] FromColorCodeCharString(string colorString)
        {
            if (colorString == null) throw new ArgumentNullException(nameof(colorString));
            List<ChatMessage> parts = new List<ChatMessage>();

            char[] chars = colorString.ToCharArray();

            ChatMessage currentSection = new ChatMessage();

            string text;
            int slice = int.MaxValue;
            int sliceStart = 0;
            int i;
            for (i = 0; i < chars.Length - 1; i++)
            {
                if (chars[i] == ChatColor.ColorChar)
                {
                    ChatColor color = ChatColor.FromChar(chars[i + 1]);
                    if (color != null)
                    {
                        if (color == ChatColor.Magic)
                        {
                            currentSection.Obfuscated = true;
                        }
                        else if (color == ChatColor.Bold)
                        {
                            currentSection.Bold = true;
                        }
                        else if (color == ChatColor.Strikethrough)
                        {
                            currentSection.Strikethrough = true;
                        }
                        else if (color == ChatColor.Underline)
                        {
                            currentSection.Underlined = true;
                        }
                        else if (color == ChatColor.Italic)
                        {
                            currentSection.Italic = true;
                        }
                        else if (color == ChatColor.Reset)
                        {
                            if (currentSection.Obfuscated == true ||
                                currentSection.Bold == true ||
                                currentSection.Strikethrough == true ||
                                currentSection.Underlined == true ||
                                currentSection.Italic == true)
                            {
                                slice = 0;
                            }
                        }
                        else if (currentSection.Color == null)
                        {
                            currentSection.Color = color;
                        }
                        else if (currentSection.Color != color)
                        {
                            slice = -2;
                        }

                        i++;
                    }

                    if (slice != int.MaxValue)
                    {
                        text = ChatColor.StripColor(colorString.Substring(sliceStart, i + 1 - sliceStart));
                        if (text.Length > 0)
                        {
                            currentSection.Text = text;
                            parts.Add(currentSection);
                            currentSection = new ChatMessage();
                        }
                        else
                        {
                            // Is memory that important?
                            currentSection.Obfuscated = null;
                            currentSection.Bold = null;
                            currentSection.Strikethrough = null;
                            currentSection.Underlined = null;
                            currentSection.Italic = null;
                            currentSection.Color = null;

                        }
                        sliceStart = i + 1; // Before next line is safe as the next 2 chars are color codes
                        i += slice;
                        slice = int.MaxValue;
                    }
                }
            }

            slice = colorString.Length - sliceStart;
            if (slice > 0)
            {
                text = ChatColor.StripColor(colorString.Substring(sliceStart, slice));
                if (text.Length > 0)
                {
                    currentSection.Text = text;
                    parts.Add(currentSection);
                }
            }

            return parts.ToArray();
        }
    }
}
