using System;
using System.Linq;

namespace Nibo.Util.Extensions
{
    public static class StringExtensions
    {
        public static string KeepOnlyNumericValue(this string @string)
        {
            if (@string.IsNullOrWhiteSpace()) { return string.Empty; }

            return new string(@string.Where(char.IsDigit).ToArray());

        }

        public static bool IsNullOrWhiteSpace(this string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }

        public static int? TryParseInt(this string @string, int startIndex, int length)
        {
            try
            {
                return int.Parse(@string.Substring(startIndex, length));
            }
            catch
            {
                return null;
            }

        }

        public static decimal? TryParseDecimal(this string @string)
        {
            try
            {
                return Convert.ToDecimal(@string);
            }
            catch
            {
                return null;
            }

        }
    }
}
