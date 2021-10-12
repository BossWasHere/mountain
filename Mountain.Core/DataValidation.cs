using Mountain.Core.Chat;
using System;
using System.Text.RegularExpressions;

namespace Mountain.Core
{
    public static class DataValidation
    {
        private static readonly Regex UsernameRegex = new Regex(@"[^A-Z0-9_]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static T AssertNotNull<T>(T obj)
        {
            return obj ?? throw new ArgumentException("Object passed is null, type " + typeof(T).Name);
        }

        public static string ValidateUsername(string username)
        {
            if (username.Length < 3 || username.Length > 16) throw new ArgumentException("A valid username must be between 3-16 characters inclusive");
            if (UsernameRegex.IsMatch(username)) throw new ArgumentException("A valid username must only contain A-Z,a-z,0-9 and _ (underscore) characters");
            return username;
        }

        public static T[] ValidateBounds<T>(T[] arr, int minIncl, int maxIncl)
        {
            if (arr.Length < minIncl) throw new ArgumentOutOfRangeException("The array is too short for the requested operation (<" + minIncl + ")");
            if (arr.Length > maxIncl) throw new ArgumentOutOfRangeException("The array is too long for the requested operation (>" + maxIncl + ")");
            return arr;
        }

        public static BaseChatMessage CheckMessageApplicable(BaseChatMessage content, bool isConnected)
        {
            //TODO
            return content;
        }
    }
}
