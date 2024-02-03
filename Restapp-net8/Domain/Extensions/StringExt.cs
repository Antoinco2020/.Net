using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class StringExt
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool ToUpperEquals(this string value, string valueEq)
        {
            return value.ToUpper().Equals(valueEq.ToUpper());
        }

        public static bool ToUpperEqualsTrim(this string value, string valueEq)
        {
            return value.ToUpper().Trim().Equals(valueEq.ToUpper().Trim());
        }

        public static bool ToUpperContains(this string value, string valueEq)
        {
            return value.ToUpper().Contains(valueEq.ToUpper());
        }

        public static string SubstringIndexOf(this string value, int start, string valueIndxOf)
        {
            return value.Substring(start, value.IndexOf(valueIndxOf));
        }

        public static string[] Split(this string value, string delimiter)
        {
            return value.Split(new char[] { delimiter.ToChar() }, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
