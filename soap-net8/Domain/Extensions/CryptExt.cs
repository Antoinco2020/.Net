using Domain.Manager;

namespace Domain.Extensions
{
    public static class CryptExt
    {
        public static Rijndael rijndael;

        public static string Encrypt(this string value)
        {
            return rijndael.Encrypt(value);
        }

        public static string Decrypt(this string value)
        {
            return rijndael.Decrypt(value);
        }
    }
}
