using System.Globalization;

namespace Nibo.Util.Extensions
{
    public static class NumberExtensions
    {
        public static string ToMoneyStringFormat(this decimal @value)
        {
            return @value.ToString("C");
        }
    }
}
