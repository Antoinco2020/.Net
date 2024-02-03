using System;
using System.Globalization;

namespace Domain.Extensions
{
    public static class DateExt
    {
        public static string FormatDate(this DateTime date, string format)
        {
            return date.ToString(format);
        }
        public static string ParseAndFormat(this string date, string format)
        {
            return DateTime.Parse(date).ToString(format);
        }

        public static string ParseExactAndFormat(this string date, string actualFormat, string format)
        {
            return DateTime.ParseExact(date, actualFormat, CultureInfo.InvariantCulture).ToString(format);
        }

        public static DateTime Parse(this string date, string format)
        {
            return DateTime.Parse(date);
        }
    }
}
