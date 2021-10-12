using Mountain.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mountain.Core
{
    /// <summary>
    /// Provides byte conversion between several data types with Big Endian byte ordering where possible.
    /// Skips use of multiple Array::Reverse and Array::Copy for greater performance and provides stream methods.
    /// Optimizations may be required in the future
    /// </summary>
    public static class DataTypes
    {
        private static readonly bool isLittleEndian = BitConverter.IsLittleEndian;

        public static byte ReadByteSafe(this Stream stream)
        {
            var b = stream.ReadByte();
            return b == -1 ? throw new EndOfStreamException() : (byte)b;
        }

        /// <summary>
        /// Converts a byte into a bool value. True if the last bit of the byte is set.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 1.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The bool representation of the next byte.</returns>
        public static bool ReadBool(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 1;
            return (bytes[offset] & 0x01) == 0x01;
        }

        /// <summary>
        /// Converts a bool into a byte array.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <returns>A byte array containing 0x01 (True) or 0x00 (False).</returns>
        public static byte[] WriteBool(bool value)
        {
            return new byte[] { value ? (byte)0x01 : (byte)0x00 };
        }

        public static bool ReadBool(this Stream stream) => stream.ReadByteSafe() == 0x01;

        public static void WriteBool(this Stream stream, bool value) => stream.WriteByte(value ? (byte)0x01 : (byte)0x00);

        /// <summary>
        /// Converts a byte pair into a short value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 2.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The short representation of the next bytes.</returns>
        public static short ReadShort(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 2;
            return (short)((bytes[offset] << 8) + bytes[offset + 1]);
        }

        /// <summary>
        /// Converts a short into a byte array.
        /// </summary>
        /// <param name="value">The short value to convert.</param>
        /// <returns>A byte array representing the short in Big Endian format.</returns>
        public static byte[] WriteShort(short value)
        {
            var bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)value;
            return bytes;
        }

        public static short ReadShort(this Stream stream)
        {
            return (short)((stream.ReadByteSafe() << 8) + stream.ReadByteSafe());
        }

        public static void WriteShort(this Stream stream, short value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Converts a byte pair into an unsigned short value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 2.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The unsigned short representation of the next bytes.</returns>
        public static ushort ReadUShort(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 2;
            return (ushort)((bytes[offset] << 8) + bytes[offset + 1]);
        }

        /// <summary>
        /// Converts an unsigned short into a byte array.
        /// </summary>
        /// <param name="value">The unsigned short value to convert.</param>
        /// <returns>A byte array representing the unsigned short in Big Endian format.</returns>
        public static byte[] WriteUShort(ushort value)
        {
            var bytes = new byte[2];
            bytes[0] = (byte)(value >> 8);
            bytes[1] = (byte)value;
            return bytes;
        }

        public static ushort ReadUShort(this Stream stream)
        {
            return (ushort)((stream.ReadByteSafe() << 8) + stream.ReadByteSafe());
        }

        public static void WriteUShort(this Stream stream, ushort value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Converts the next four bytes into a int value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 4.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The int representation of the next bytes.</returns>
        public static int ReadInt(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 4;
            return (bytes[offset] << 24) + (bytes[offset + 1] << 16) + (bytes[offset + 2] << 8) + bytes[offset + 3];
        }

        /// <summary>
        /// Converts a int into a byte array.
        /// </summary>
        /// <param name="value">The int value to convert.</param>
        /// <returns>A byte array representing the int in Big Endian format.</returns>
        public static byte[] WriteInt(int value)
        {
            var bytes = new byte[4];
            bytes[0] = (byte)(value >> 24);
            bytes[1] = (byte)(value >> 16);
            bytes[2] = (byte)(value >> 8);
            bytes[3] = (byte)value;
            return bytes;
        }

        public static int ReadInt(this Stream stream)
        {
            return ((stream.ReadByteSafe() << 24) + (stream.ReadByteSafe() << 16) + (stream.ReadByteSafe() << 8) + stream.ReadByteSafe());
        }

        public static void WriteInt(this Stream stream, int value)
        {
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }



        /// <summary>
        /// Converts the next four bytes into an unsigned int value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 4.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The uint representation of the next bytes.</returns>
        public static uint ReadUInt(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 4;
            return (uint)((bytes[offset] << 24) + (bytes[offset + 1] << 16) + (bytes[offset + 2] << 8) + bytes[offset + 3]);
        }

        /// <summary>
        /// Converts an unsigned int into a byte array.
        /// </summary>
        /// <param name="value">The int value to convert.</param>
        /// <returns>A byte array representing the int in Big Endian format.</returns>
        public static byte[] WriteUInt(uint value)
        {
            var bytes = new byte[4];
            bytes[0] = (byte)(value >> 24);
            bytes[1] = (byte)(value >> 16);
            bytes[2] = (byte)(value >> 8);
            bytes[3] = (byte)value;
            return bytes;
        }

        public static uint ReadUInt(this Stream stream)
        {
            return (uint)((stream.ReadByteSafe() << 24) + (stream.ReadByteSafe() << 16) + (stream.ReadByteSafe() << 8) + stream.ReadByteSafe());
        }

        public static void WriteUInt(this Stream stream, uint value)
        {
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Converts the next 8 bytes into a long value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 8.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The long representation of the next bytes.</returns>
        public static long ReadLong(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 8;
            long value = ((long)bytes[offset++] << 56)
            + ((long)bytes[offset++] << 48)
            + ((long)bytes[offset++] << 40)
            + ((long)bytes[offset++] << 32)
            + ((long)bytes[offset++] << 24)
            + ((long)bytes[offset++] << 16)
            + ((long)bytes[offset++] << 8)
            + bytes[offset];

            return value;
        }

        /// <summary>
        /// Converts a long into a byte array.
        /// </summary>
        /// <param name="value">The long value to convert.</param>
        /// <returns>A byte array representing the long in Big Endian format.</returns>
        public static byte[] WriteLong(long value)
        {
            var bytes = new byte[8];
            bytes[0] = (byte)(value >> 56);
            bytes[1] = (byte)(value >> 48);
            bytes[2] = (byte)(value >> 40);
            bytes[3] = (byte)(value >> 32);
            bytes[4] = (byte)(value >> 24);
            bytes[5] = (byte)(value >> 16);
            bytes[6] = (byte)(value >> 8);
            bytes[7] = (byte)value;
            return bytes;
        }

        public static long ReadLong(this Stream stream)
        {
            var buf = new byte[8];
            stream.Read(buf);
            return ReadLong(buf, out _);
        }

        public static void WriteLong(this Stream stream, long value)
        {
            stream.WriteByte((byte)(value >> 56));
            stream.WriteByte((byte)(value >> 48));
            stream.WriteByte((byte)(value >> 40));
            stream.WriteByte((byte)(value >> 32));
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Converts the next 8 bytes into an unsigned long value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 8.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The unsigned long representation of the next bytes.</returns>
        public static ulong ReadULong(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 8;
            ulong value = ((ulong)bytes[offset++] << 56)
            + ((ulong)bytes[offset++] << 48)
            + ((ulong)bytes[offset++] << 40)
            + ((ulong)bytes[offset++] << 32)
            + ((ulong)bytes[offset++] << 24)
            + ((ulong)bytes[offset++] << 16)
            + ((ulong)bytes[offset++] << 8)
            + bytes[offset];

            return value;
        }

        /// <summary>
        /// Converts an unsigned long into a byte array.
        /// </summary>
        /// <param name="value">The unsigned long value to convert.</param>
        /// <returns>A byte array representing the unsigned long in Big Endian format.</returns>
        public static byte[] WriteULong(ulong value)
        {
            var bytes = new byte[8];
            bytes[0] = (byte)(value >> 56);
            bytes[1] = (byte)(value >> 48);
            bytes[2] = (byte)(value >> 40);
            bytes[3] = (byte)(value >> 32);
            bytes[4] = (byte)(value >> 24);
            bytes[5] = (byte)(value >> 16);
            bytes[6] = (byte)(value >> 8);
            bytes[7] = (byte)value;
            return bytes;
        }

        public static ulong ReadULong(this Stream stream)
        {
            var buf = new byte[8];
            stream.Read(buf);
            return ReadULong(buf, out _);
        }

        public static void WriteULong(this Stream stream, ulong value)
        {
            stream.WriteByte((byte)(value >> 56));
            stream.WriteByte((byte)(value >> 48));
            stream.WriteByte((byte)(value >> 40));
            stream.WriteByte((byte)(value >> 32));
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        /// <summary>
        /// Converts the next four bytes into an IEEE-754 single precision floating point value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 4.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The float representation of the next bytes.</returns>
        public static float ReadFloat(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 4;
            if (!isLittleEndian) return BitConverter.ToSingle(bytes, offset);
            offset += 3;
            var leConversion = new byte[]
            {
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset]
            };
            return BitConverter.ToSingle(leConversion);
        }

        /// <summary>
        /// Converts an IEE-754 single precision floating point value into a byte array.
        /// </summary>
        /// <param name="value">The float value to convert.</param>
        /// <returns>A byte array representing the float in Big Endian format.</returns>
        public static byte[] WriteFloat(float value)
        {
            var floatBytes = BitConverter.GetBytes(value);

            if (isLittleEndian)
            {
                var bytes = new byte[4];

                bytes[0] = floatBytes[3];
                bytes[1] = floatBytes[2];
                bytes[2] = floatBytes[1];
                bytes[3] = floatBytes[0];

                return bytes;
            }
            return floatBytes;
        }

        public static float ReadFloat(this Stream stream)
        {
            var buf = new byte[4];
            stream.Read(buf);
            return ReadFloat(buf, out _);
        }

        public static void WriteFloat(this Stream stream, float value) => stream.Write(WriteFloat(value));

        /// <summary>
        /// Converts the next eight bytes into an IEEE-754 double precision floating point value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 8.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <remarks>Big Endian byte ordering is used.</remarks>
        /// <returns>The double representation of the next bytes.</returns>
        public static double ReadDouble(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 8;
            if (!isLittleEndian) return BitConverter.ToDouble(bytes, offset);
            offset += 7;
            var leConversion = new byte[]
            {
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset--],
                bytes[offset],
            };
            return BitConverter.ToDouble(leConversion);
        }

        /// <summary>
        /// Converts an IEE-754 double precision floating point value into a byte array.
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <returns>A byte array representing the double in Big Endian format.</returns>
        public static byte[] WriteDouble(double value)
        {
            var doubleBytes = BitConverter.GetBytes(value);

            if (isLittleEndian)
            {
                var bytes = new byte[8];

                bytes[0] = doubleBytes[7];
                bytes[1] = doubleBytes[6];
                bytes[2] = doubleBytes[5];
                bytes[3] = doubleBytes[4];
                bytes[4] = doubleBytes[3];
                bytes[5] = doubleBytes[2];
                bytes[6] = doubleBytes[1];
                bytes[7] = doubleBytes[0];

                return bytes;
            }

            return doubleBytes;
        }

        public static double ReadDouble(this Stream stream)
        {
            var buf = new byte[8];
            stream.Read(buf);
            return ReadDouble(buf, out _);
        }

        public static void WriteDouble(this Stream stream, double value) => stream.Write(WriteDouble(value));

        /// <summary>
        /// Converts a variable length number of bytes into a int value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <exception cref="Mountain.Exceptions.DataReadException">Thrown when the VarInt is incorrectly terminated (too large).</exception>
        /// <returns>The int representation of the next bytes.</returns>
        public static int ReadVarInt(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 0;
            int res = 0;
            byte read;
            do
            {
                read = bytes[offset++];
                int value = (read & 0b01111111);
                res |= (value << (7 * length));

                length++;
                if (length > 5)
                {
                    throw new DataReadException("VarInt is too big");
                }

            } while ((read & 0b10000000) != 0);

            return res;
        }

        /// <summary>
        /// Converts a 32-bit integer into a variable length number byte array.
        /// </summary>
        /// <param name="value">The int value to convert</param>
        /// <returns>A byte array representing the int as a VarInt.</returns>
        public static byte[] WriteVarInt(int value)
        {
            var uval = (uint)value;

            int length = 0;
            byte[] bytes = new byte[5];
            do
            {
                byte temp = (byte)(uval & 0b01111111);
                uval >>= 7;
                if (uval != 0)
                {
                    temp |= 0b10000000;
                }
                bytes[length++] = temp;
            } while (uval != 0);

            byte[] shortened = new byte[length];
            for (int i = 0; i < length; i++) shortened[i] = bytes[i];
            return shortened;
        }

        public static int ReadVarInt(this Stream stream)
        {
            return stream.ReadVarInt(out _);
        }

        public static int ReadVarInt(this Stream stream, out int length)
        {
            length = 0;
            int res = 0;
            byte read;
            do
            {
                read = stream.ReadByteSafe();
                int value = (read & 0b01111111);
                res |= (value << (7 * length));

                length++;
                if (length > 5)
                {
                    throw new DataReadException("VarInt is too big");
                }

            } while ((read & 0b10000000) != 0);

            return res;
        }

        public static void WriteVarInt(this Stream stream, int value)
        {
            var uval = (uint)value;

            do
            {
                byte temp = (byte)(uval & 0b01111111);
                uval >>= 7;
                if (uval != 0)
                {
                    temp |= 0b10000000;
                }
                stream.WriteByte(temp);
            } while (uval != 0);
        }

        /// <summary>
        /// Converts a variable length number of bytes into a long value.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <exception cref="Mountain.Exceptions.DataReadException">Thrown when the VarLong is incorrectly terminated (too large).</exception>
        /// <returns>The long representation of the next bytes.</returns>
        public static long ReadVarLong(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            length = 0;
            long res = 0;
            byte read;
            do
            {
                read = bytes[offset++];
                long value = (read & 0b01111111);
                res |= (value << (7 * length));

                length++;
                if (length > 10)
                {
                    throw new DataReadException("VarLong is too big");
                }

            } while ((read & 0b10000000) != 0);

            return res;
        }

        /// <summary>
        /// Converts a 64-bit integer into a variable length number byte array.
        /// </summary>
        /// <param name="value">The long value to convert.</param>
        /// <returns>A byte array representing the long as a VarLong.</returns>
        public static byte[] WriteVarLong(long value)
        {
            var uval = (ulong)value;

            int length = 0;
            byte[] bytes = new byte[10];
            do
            {
                byte temp = (byte)(uval & 0b01111111);
                uval >>= 7;
                if (uval != 0)
                {
                    temp |= 0b10000000;
                }
                bytes[length++] = temp;
            } while (uval != 0);
            
            byte[] shortened = new byte[length];
            for (int i = 0; i < length; i++) shortened[i] = bytes[i];
            return shortened;
        }

        public static long ReadVarLong(this Stream stream)
        {
            return stream.ReadVarLong(out _);
        }

        public static long ReadVarLong(this Stream stream, out int length)
        {
            length = 0;
            long res = 0;
            byte read;
            do
            {
                read = stream.ReadByteSafe();
                long value = (read & 0b01111111);
                res |= (value << (7 * length));

                length++;
                if (length > 10)
                {
                    throw new DataReadException("VarLong is too big");
                }

            } while ((read & 0b10000000) != 0);

            return res;
        }

        public static void WriteVarLong(this Stream stream, long value)
        {
            var uval = (ulong)value;

            do
            {
                byte temp = (byte)(uval & 0b01111111);
                uval >>= 7;
                if (uval != 0)
                {
                    temp |= 0b10000000;
                }
                stream.WriteByte(temp);
            } while (uval != 0);
        }

        /// <summary>
        /// Converts a variable length number of bytes prefixed by a VarInt value into a string.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <exception cref="Mountain.Exceptions.DataReadException">Thrown when the encoded string length is invalid.</exception>
        /// <returns>The string representation of the next bytes.</returns>
        public static string ReadVarString(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int strLength = ReadVarInt(bytes, out int intLength, offset);
            length = intLength + strLength;
            return Encoding.UTF8.GetString(bytes, intLength + offset, strLength);
        }

        /// <summary>
        /// Converts a string into a variable length-prefixed UTF-8 byte array.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>A byte array representing the string as a VarString.</returns>
        public static byte[] WriteVarString(string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            int sbc = stringBytes.Length;

            byte[] lengthBytes = WriteVarInt(sbc);
            return Join(lengthBytes, stringBytes);
        }

        public static string ReadVarString(this Stream stream)
        {
            return stream.ReadVarString(out _);
        }

        public static string ReadVarString(this Stream stream, out int length)
        {
            int strLen = stream.ReadVarInt(out length);
            length += strLen;
            var buf = new byte[strLen];
            stream.Read(buf);
            return Encoding.UTF8.GetString(buf);
        }

        public static void WriteVarString(this Stream stream, string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);

            stream.WriteVarInt(stringBytes.Length);
            stream.Write(stringBytes);
        }

        /// <summary>
        /// Converts a variable length number of bytes prefixed by a short value into a string.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The string representation of the next bytes.</returns>
        public static string ReadShortString(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            ushort strLength = ReadUShort(bytes, out int shortLength, offset);
            length = shortLength + strLength;
            return Encoding.UTF8.GetString(bytes, shortLength + offset, strLength);
        }

        /// <summary>
        /// Converts a string into a 16-bit unsigned length-prefixed UTF-8 byte array.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <returns>A byte array representing the string with a 16-bit unsigned length prefix.</returns>
        public static byte[] WriteShortString(string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            int sbc = stringBytes.Length;

            byte[] lengthBytes = WriteUShort((ushort)sbc);
            return Join(lengthBytes, stringBytes);
        }

        public static string ReadShortString(this Stream stream)
        {
            ushort strLen = stream.ReadUShort();
            var buf = new byte[strLen];
            stream.Read(buf);
            return Encoding.UTF8.GetString(buf);
        }

        public static void WriteShortString(this Stream stream, string value)
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
            stream.WriteUShort((ushort)stringBytes.Length);

            stream.Write(stringBytes);
        }

        /// <summary>
        /// Converts a byte to a specific enum mapping by the specified type parameter.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to be 1.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <exception cref="System.InvalidCastException">Thrown when the byte value cannot be cast to the specified type.</exception>
        /// <returns>The enum representation of the byte.</returns>
        public static T ReadEnumByte<T>(byte[] bytes, out int length, int offset = 0) where T : Enum
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            object o = (int)bytes[offset];
            length = 1;
            return (T)o;
        }

        /// <summary>
        /// Converts an enum constant into its numerical form as a byte.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>A byte array representing the enum constant by its numerical value as a single byte.</returns>
        public static byte[] WriteEnumByte<T>(T value) where T : Enum
        {
            return new byte[] { (byte)(object)value };
        }

        public static T ReadEnumByte<T>(this Stream stream)
        {
            object o = (int)stream.ReadByteSafe();
            return (T)o;
        }

        public static void WriteEnumByte<T>(this Stream stream, T value) where T : Enum
        {
            stream.WriteByte((byte)(object)value);
        }

        /// <summary>
        /// Converts a variable length number of bytes into a int value and casts it to the generic enum type.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <exception cref="System.InvalidCastException">Thrown when the byte value cannot be cast to the specified type.</exception>
        /// <returns>The enum representation of the VarInt.</returns>
        public static T ReadEnumVarInt<T>(byte[] bytes, out int length, int offset = 0) where T : Enum
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            object o = ReadVarInt(bytes, out length, offset);
            return (T)o;
        }

        /// <summary>
        /// Converts an enum constant into its numerical form as a byte.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>A byte array representing the enum constant by its numerical value as a single byte.</returns>
        public static byte[] WriteEnumVarInt<T>(T value) where T : Enum
        {
            return WriteVarInt((int)(object)value);
        }

        public static T ReadEnumVarInt<T>(this Stream stream)
        {
            object o = stream.ReadVarInt();
            return (T)o;
        }

        public static void WriteEnumVarInt<T>(this Stream stream, T value) where T : Enum
        {
            stream.WriteVarInt((int)(object)value);
        }

        /// <summary>
        /// Converts an int prefixed array of bytes into a byte array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The new byte array</returns>
        public static byte[] ReadIntPrefixedByteArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int byteCount = ReadInt(bytes, out int byteIntLength, offset);

            offset += byteIntLength;
            length = byteIntLength + byteCount;

            byte[] newBytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                newBytes[i] = bytes[offset++];
            }
            return newBytes;
        }

        /// <summary>
        /// Converts an array of bytes into an int prefixed array of bytes
        /// </summary>
        /// <param name="value">The array of bytes to convert.</param>
        /// <returns>A byte array representing the prefixed byte array</returns>
        public static byte[] WriteIntPrefixedByteArray(byte[] value)
        {
            var arrLenB = WriteInt(value.Length);
            var newBytes = CopyWithExtraCapacity(arrLenB, value.Length, 0);
            for (int i = 0; i < value.Length; i++)
            {
                newBytes[i + arrLenB.Length] = value[i];
            }

            return newBytes;
        }

        public static byte[] ReadIntPrefixedByteArray(this Stream stream)
        {
            int count = stream.ReadInt();
            byte[] buf = new byte[count];
            stream.Read(buf);
            return buf;
        }

        public static void WriteIntPrefixedByteArray(this Stream stream, byte[] value)
        {
            stream.WriteInt(value.Length);
            stream.Write(value);
        }

        /// <summary>
        /// Converts an int prefixed array of ints into a int array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The new int array</returns>
        public static int[] ReadIntPrefixedIntArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int intCount = ReadInt(bytes, out int intIntLength, offset);

            offset += intIntLength;
            length = intIntLength + intCount * 4;

            int[] ints = new int[intCount];
            for (int i = 0; i < intCount; i++)
            {
                ints[i] = ReadInt(bytes, out _, offset);
                offset += 4;
            }
            return ints;
        }

        /// <summary>
        /// Converts an array of bytes into an int prefixed array of ints
        /// </summary>
        /// <param name="value">The array of ints to convert.</param>
        /// <returns>A byte array representing the prefixed int array</returns>
        public static byte[] WriteIntPrefixedIntArray(int[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteIntPrefixedIntArray(value);

            return stream.ToArray();
        }

        public static int[] ReadIntPrefixedIntArray(this Stream stream)
        {
            int count = stream.ReadInt();
            int[] ints = new int[count];
            for (int i = 0; i < count; i++)
            {
                ints[i] = stream.ReadInt();
            }
            return ints;
        }

        public static void WriteIntPrefixedIntArray(this Stream stream, int[] value)
        {
            stream.WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteInt(value[i]);
            }
        }

        /// <summary>
        /// Converts an int prefixed array of longs into a long array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The new long array</returns>
        public static long[] ReadIntPrefixedLongArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int longCount = ReadInt(bytes, out int longIntLength, offset);

            offset += longIntLength;
            length = longIntLength + longCount * 8;

            long[] longs = new long[longCount];
            for (int i = 0; i < longCount; i++)
            {
                longs[i] = ReadLong(bytes, out _, offset);
                offset += 8;
            }
            return longs;
        }

        /// <summary>
        /// Converts an array of bytes into an int prefixed array of longs
        /// </summary>
        /// <param name="value">The array of longs to convert.</param>
        /// <returns>A byte array representing the prefixed long array</returns>
        public static byte[] WriteIntPrefixedLongArray(long[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteIntPrefixedLongArray(value);

            return stream.ToArray();
        }

        public static long[] ReadIntPrefixedLongArray(this Stream stream)
        {
            int count = stream.ReadInt();
            long[] longs = new long[count];
            for (int i = 0; i < count; i++)
            {
                longs[i] = stream.ReadLong();
            }
            return longs;
        }

        public static void WriteIntPrefixedLongArray(this Stream stream, long[] value)
        {
            stream.WriteInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteLong(value[i]);
            }
        }

        /// <summary>
        /// Converts a VarInt prefixed array of bytes into a byte array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The byte array represented by the bytes</returns>
        public static byte[] ReadVarIntPrefixedByteArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int byteCount = ReadVarInt(bytes, out int byteIntLength, offset);

            offset += byteIntLength;
            length = byteIntLength + byteCount;

            byte[] newBytes = new byte[byteCount];
            for (int i = 0; i < byteCount; i++)
            {
                newBytes[i] = bytes[offset++];
            }
            return newBytes;
        }

        /// <summary>
        /// Converts an array of byte into a VarInt prefixed array of bytes
        /// </summary>
        /// <param name="value">The array of bytes to convert.</param>
        /// <returns>A byte array representing the prefixed byte array</returns>
        public static byte[] WriteVarIntPrefixedByteArray(byte[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteVarIntPrefixedByteArray(value);

            return stream.ToArray();
        }

        public static byte[] ReadVarIntPrefixedByteArray(this Stream stream)
        {
            int count = stream.ReadVarInt();
            byte[] bytes = new byte[count];
            stream.Read(bytes);
            return bytes;
        }

        public static void WriteVarIntPrefixedByteArray(this Stream stream, byte[] value)
        {
            stream.WriteVarInt(value.Length);
            stream.Write(value);
        }

        /// <summary>
        /// Converts a VarInt prefixed array of ints into a int array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The int array represented by the bytes</returns>
        public static int[] ReadVarIntPrefixedVarIntArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int intCount = ReadVarInt(bytes, out int intIntLength, offset);

            offset += intIntLength;
            length = intIntLength;

            int[] ints = new int[intCount];
            for (int i = 0; i < intCount; i++)
            {
                ints[i] = ReadVarInt(bytes, out int varIntLength, offset);
                offset += varIntLength;
                length += varIntLength;
            }
            return ints;
        }

        /// <summary>
        /// Converts an array of int into a VarInt prefixed array of ints
        /// </summary>
        /// <param name="value">The array of ints to convert.</param>
        /// <returns>A byte array representing the prefixed int array</returns>
        public static byte[] WriteVarIntPrefixedVarIntArray(int[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteVarIntPrefixedVarIntArray(value);

            return stream.ToArray();
        }

        public static int[] ReadVarIntPrefixedVarIntArray(this Stream stream)
        {
            int count = stream.ReadVarInt();
            int[] ints = new int[count];
            for (int i = 0; i < count; i++)
            {
                ints[i] = stream.ReadVarInt();
            }
            return ints;
        }

        public static void WriteVarIntPrefixedVarIntArray(this Stream stream, int[] value)
        {
            stream.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteVarInt(value[i]);
            }
        }



        /// <summary>
        /// Converts a VarInt prefixed array of longs into a long array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The long array represented by the bytes</returns>
        public static long[] ReadVarIntPrefixedLongArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int longCount = ReadVarInt(bytes, out int intIntLength, offset);

            offset += intIntLength;
            length = intIntLength;

            long[] longs = new long[longCount];
            for (int i = 0; i < longCount; i++)
            {
                longs[i] = ReadLong(bytes, out _, offset);
                offset += 8;
                length += 8;
            }
            return longs;
        }

        /// <summary>
        /// Converts an array of long into a VarInt prefixed array of longs
        /// </summary>
        /// <param name="value">The array of longs to convert.</param>
        /// <returns>A byte array representing the prefixed long array</returns>
        public static byte[] WriteVarIntPrefixedLongArray(long[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteVarIntPrefixedLongArray(value);

            return stream.ToArray();
        }

        public static long[] ReadVarIntPrefixedLongArray(this Stream stream)
        {
            int count = stream.ReadVarInt();
            long[] longs = new long[count];
            for (int i = 0; i < count; i++)
            {
                longs[i] = stream.ReadLong();
            }
            return longs;
        }

        public static void WriteVarIntPrefixedLongArray(this Stream stream, long[] value)
        {
            stream.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteLong(value[i]);
            }
        }

        /// <summary>
        /// Converts a VarInt prefixed array of longs into a long array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The long array represented by the bytes</returns>
        public static long[] ReadVarIntPrefixedVarLongArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int longCount = ReadInt(bytes, out int longIntLength, offset);

            offset += longIntLength;
            length = longIntLength;

            long[] longs = new long[longCount];
            for (int i = 0; i < longCount; i++)
            {
                longs[i] = ReadVarLong(bytes, out int varLongLength, offset);
                offset += varLongLength;
                length += varLongLength;
            }
            return longs;
        }

        /// <summary>
        /// Converts an array of long into a VarInt prefixed array of longs
        /// </summary>
        /// <param name="value">The array of strings to convert.</param>
        /// <returns>A byte array representing the prefixed long array</returns>
        public static byte[] WriteVarIntPrefixedVarLongArray(long[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteVarIntPrefixedVarLongArray(value);

            return stream.ToArray();
        }

        public static long[] ReadVarIntPrefixedVarLongArray(this Stream stream)
        {
            int count = stream.ReadVarInt();
            long[] longs = new long[count];
            for (int i = 0; i < count; i++)
            {
                longs[i] = stream.ReadVarLong();
            }
            return longs;
        }

        public static void WriteVarIntPrefixedVarLongArray(this Stream stream, long[] value)
        {
            stream.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteVarLong(value[i]);
            }
        }

        /// <summary>
        /// Converts a VarInt prefixed array of VarStrings into a string array
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The string array represented by the bytes</returns>
        public static string[] ReadVarIntPrefixedStringArray(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            int stringCount = ReadVarInt(bytes, out int strIntLength, offset);

            string[] strings = new string[stringCount];
            length = strIntLength;
            for (int i = 0; i < stringCount; i++)
            {
                strings[i] = ReadVarString(bytes, out int perStrLength, offset + length);
                length += perStrLength;
            }
            return strings;
        }

        /// <summary>
        /// Converts an array of strings into a VarInt prefixed array of VarStrings
        /// </summary>
        /// <param name="value">The array of strings to convert.</param>
        /// <returns>A byte array representing the prefixed string array</returns>
        public static byte[] WriteVarIntPrefixedStringArray(string[] value)
        {
            using var stream = new MemoryStream();
            stream.WriteVarIntPrefixedStringArray(value);

            return stream.ToArray();
        }

        public static string[] ReadVarIntPrefixedStringArray(this Stream stream)
        {
            int count = stream.ReadVarInt();
            string[] strings = new string[count];
            for (int i = 0; i < count; i++)
            {
                strings[i] = stream.ReadVarString();
            }
            return strings;
        }

        public static void WriteVarIntPrefixedStringArray(this Stream stream, string[] value)
        {
            stream.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                stream.WriteVarString(value[i]);
            }
        }

        /// <summary>
        /// Converts the next 128-bit into a Uuid, with the most significant bits first.
        /// </summary>
        /// <param name="bytes">The source byte array.</param>
        /// <param name="length">How many bytes were read. Guaranteed to return 16.</param>
        /// <param name="offset">The index to begin reading from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the byte array is null.</exception>
        /// <exception cref="System.IndexOutOfRangeException">Thrown when the offset is out of the range of the array.</exception>
        /// <returns>The Uuid representation of the next bytes.</returns>
        /// <seealso cref="Mountain.Uuid"/>
        public static Uuid ReadUuid(byte[] bytes, out int length, int offset = 0)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            long msb = ReadLong(bytes, out _, offset);
            long lsb = ReadLong(bytes, out _, offset + 8);
            length = 16;
            return new Uuid(lsb, msb);
        }

        /// <summary>
        /// Converts a Uuid into a 128-bit integer. The most significant bits are written first.
        /// </summary>
        /// <param name="value">The Uuid value to convert.</param>
        /// <returns>A byte array representing the 128-bit Uuid</returns>
        /// <seealso cref="Mountain.Uuid"/>
        public static byte[] WriteUuid(Uuid value)
        {
            var msb = WriteLong(value.MostSignificantBits);
            var lsb = WriteLong(value.LeastSignificantBits);

            return Join(msb, lsb);
        }

        public static Uuid ReadUuid(this Stream stream)
        {
            long msb = stream.ReadLong();
            long lsb = stream.ReadLong();
            return new Uuid(lsb, msb);
        }

        public static void WriteUuid(this Stream stream, Uuid value)
        {
            stream.WriteLong(value.MostSignificantBits);
            stream.WriteLong(value.LeastSignificantBits);
        }

        public static byte[] Join(params byte[][] arrays)
        {
            int length = 0;
            foreach (byte[] arr in arrays) length += arr.Length;
            var newArr = new byte[length];

            int i = 0;
            foreach (byte[] arr in arrays)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    newArr[i++] = arr[j];
                }
            }

            return newArr;
        }

        public static byte[] CopyWithExtraCapacity(byte[] arr, int extraCapacity, int offset = 0)
        {
            int length = arr.Length;
            var bytes = new byte[length + extraCapacity];
            offset = offset < 0 || offset > extraCapacity ? 0 : offset;
            Array.Copy(arr, 0, bytes, offset, length);
            return bytes;
        }
    }
}
