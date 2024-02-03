using Domain.Manager;

namespace Domain.Extensions
{
    public static class ResourceExt
    {
        public static ResourceFileManager resFileManager;

        public static string GetConfig(this string value)
        {
            return resFileManager.GetConfigData(value);
        }

        public static bool SetConfig(this string key, string value)
        {
            return resFileManager.SetConfigData(key, value);
        }

        public static string GetConfigUpper(this string value)
        {
            return value.GetConfig().ToUpper();
        }

        public static string GetConfigLower(this string value)
        {
            return value.GetConfig().ToLower();
        }

        public static string GetConfigTrim(this string value)
        {
            return value.GetConfig().Trim();
        }

        public static string GetConfigUpperTrim(this string value)
        {
            return value.GetConfig().ToUpper().Trim();
        }

        public static string GetConfigReplace(this string value, char oldValue, char newValue)
        {
            return value.GetConfig().Replace(oldValue, newValue);
        }

        public static short GetConfigI16(this string value)
        {
            return value.GetConfig().ToInt16();
        }

        public static int GetConfigInt(this string value)
        {
            return value.GetConfig().ToInt();
        }

        public static string[] GetConfigSplit(this string value, char valueToSplit)
        {
            return resFileManager.GetConfigData(value).Split(new char[] { valueToSplit }, System.StringSplitOptions.RemoveEmptyEntries);
        }


    }
}
