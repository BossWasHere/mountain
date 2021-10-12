using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Mountain.Mojang
{
    public static class CryptoUtils
    {
        public static string ModifiedSha1Digest(string data)
        {
            var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(data));
            Array.Reverse(hash);

            var bigInt = new BigInteger(hash);
            return bigInt < 0 ? "-" + (-bigInt).ToString("x").TrimStart('0') : bigInt.ToString("x").TrimStart('0');
        }
    }
}
