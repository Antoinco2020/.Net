
namespace Domain.Extensions
{
    public static class ConvertExt
    {
        /// <summary>
        /// Convert a string value to Int16 (short)
        /// </summary>
        public static short ToInt16(this string value)
        {
            short result = short.MinValue;
            short.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Convert a string value to int
        /// </summary>
        public static int ToInt(this string value)
        {
            int result = int.MinValue;
            int.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Convert a string value to long
        /// </summary>
        public static long ToInt64(this string value)
        {
            long result = long.MinValue;
            long.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Convert a string value to char
        /// </summary>
        public static char ToChar(this string value)
        {
            return char.Parse(value);
        }

        /// <summary>
        /// Convert a integer value to Int16 (short)
        /// </summary>
        public static short ToInt16(this int value)
        {
            return (short)value;
        }

    }
}
