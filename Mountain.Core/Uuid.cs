using Mountain.Core.Serializers;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Mountain.Core
{
    [JsonConverter(typeof(JsonUuidFormatter))]
    public struct Uuid : IEquatable<Uuid>
    {
        public long LeastSignificantBits { get; }
        public long MostSignificantBits { get; }

        public Uuid(long leastSignificantBits, long mostSignificantBits)
        {
            LeastSignificantBits = leastSignificantBits;
            MostSignificantBits = mostSignificantBits;
        }

        public static Uuid NewUUID()
        {
            return FromGuid(Guid.NewGuid());
        }

        public static Uuid NewOfflineUuid(string name)
        {
            var inputString = "OfflinePlayer:" + name;
            var bytes = Encoding.UTF8.GetBytes(inputString);

            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(bytes);

            hash[6] = (byte)((hash[6] & 0x0F) | 0x30);
            hash[8] = (byte)((hash[8] & 0x3F) | 0x80);

            return DataTypes.ReadUuid(hash, out _);
        }

        public override bool Equals(object obj)
        {
            return obj is Uuid uuidObj && Equals(uuidObj);
        }

        public bool Equals(Uuid uuid)
        {
            return uuid.LeastSignificantBits == LeastSignificantBits && uuid.MostSignificantBits == MostSignificantBits;
        }

        public static bool operator ==(Uuid a, Uuid b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Uuid a, Uuid b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return ToGuid().GetHashCode();
        }

        public Guid ToGuid()
        {
            if (this == default)
            {
                return default;
            }

            byte[] msb = BitConverter.GetBytes(MostSignificantBits);
            byte[] lsb = BitConverter.GetBytes(LeastSignificantBits);
            byte[] guidBytes = new byte[16] {
                msb[4],
                msb[5],
                msb[6],
                msb[7],
                msb[2],
                msb[3],
                msb[0],
                msb[1],
                lsb[7],
                lsb[6],
                lsb[5],
                lsb[4],
                lsb[3],
                lsb[2],
                lsb[1],
                lsb[0]
            };

            return new Guid(guidBytes);
        }

        public static Uuid FromGuid(Guid guid)
        {
            if (guid == default)
            {
                return default;
            }

            byte[] guidBytes = guid.ToByteArray();
            byte[] uuidBytes = new byte[16] {
                guidBytes[15],
                guidBytes[14],
                guidBytes[13],
                guidBytes[12],
                guidBytes[11],
                guidBytes[10],
                guidBytes[9],
                guidBytes[8],
                guidBytes[6],
                guidBytes[7],
                guidBytes[4],
                guidBytes[5],
                guidBytes[0],
                guidBytes[1],
                guidBytes[2],
                guidBytes[3]
            };

            return new Uuid(BitConverter.ToInt64(uuidBytes, 0), BitConverter.ToInt64(uuidBytes, 8));
        }

        public static Uuid FromString(string uuid)
        {
            return FromGuid(Guid.Parse(uuid));
        }

        public override string ToString()
        {
            return ToGuid().ToString();
        }

        public string ToDehyphenatedString()
        {
            return new string(ToString().Where(x => x != '-').ToArray());
        }
    }
}
