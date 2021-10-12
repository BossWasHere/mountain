using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mountain.Core.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace MountainTest
{
    [TestClass]
    public class ChatTest
    {
        private const string Message0 = "Test string";
        private const string Message1 = "§1Test string";
        private const string Message2 = "§1§lTest string";
        private const string Message3 = "§1§lTest §3string";
        private const string Message4 = "§1§lTest §k§r§3string";
        private const string Message5 = "§1§lTest §k§r§3string§3§3";
        private const string Message6 = "§1§lTest §k§r§3string§3§l";
        private const string Message7 = "§1§lTest §k§r§3string§§3§a";
        private const string Message8 = "Test §k§r§3string§3§l§r";

        [TestMethod]
        public void TestMessageDecoding()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ChatMessage.FromColorCodeCharString(null));
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message0), Message0);
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message1), Message0);
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message2), Message0);
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message3), "Test ", "string");
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message4), "Test ", "string");
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message5), "Test ", "string");
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message6), "Test ", "string");
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message7), "Test ", "string§");
            MessageAssertion(ChatMessage.FromColorCodeCharString(Message8), "Test ", "string");
        }

        private void MessageAssertion(ChatMessage[] messages, params string[] texts)
        {
            Assert.AreEqual(texts.Length, messages.Length);
            for (int i = 0; i < messages.Length; i++)
            {
                Assert.AreEqual(texts[i], messages[i].Text);
            }
        }
    }
}
