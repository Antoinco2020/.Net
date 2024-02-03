using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Manager
{
    public static class Enums
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

        public static string GetDescription<T>(this T e)
        {
            string str = (string)null;

            if ((object)e is Enum)
            {
                Type type = e.GetType();
                foreach (int num in Enum.GetValues(type))
                {
                    if (num == Convert.ToInt32(e, CultureInfo.InvariantCulture))
                    {
                        object[] customAttributes = type.GetMember(type.GetEnumName((object)num))[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if ((uint)customAttributes.Length > 0U)
                        {
                            str = ((DescriptionAttribute)customAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return str;
        }

        public enum enumLogType
        {
            SERILOG,
            NLOG
        }

        public enum enumLogOutputType
        {
            FILE,
            CONSOLE
        }

        public enum enumMailType
        {
            ND,
            PEO,
            PEC
        }

        public enum enumLogLevel
        {
            [Description("DEBUG")]
            DEBUG,
            [Description("INFO")]
            INFO,
            [Description("WARNING")]
            WARNING,
            [Description("ERROR")]
            ERROR
        }

    }
}
