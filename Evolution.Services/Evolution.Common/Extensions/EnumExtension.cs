using System;
using System.ComponentModel.DataAnnotations;

namespace Evolution.Common.Extensions
{
    public static class EnumExtension
    {
        public static string ToId<T>(this T source) where T : IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return ((int)(IConvertible)source).ToString();
        }

        public static string DisplayName(this Enum value)
        {
            return EnumExtension.GetDisplayName(value);
        }

        public static int? ToIdByDisplayName<T>(string displayName) where T : struct, IConvertible
        {
            int? result = null;
            if (!typeof(T).IsEnum || string.IsNullOrEmpty(displayName))
                return result;

            foreach (var value in Enum.GetValues(typeof(T)))
            {
                if (EnumExtension.GetDisplayName(value).Trim() == displayName)
                    result = ((int)(IConvertible)value);
            }
            return result;
        }

        public static string FirstChar(this Enum value)
        {
            var enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            return enumValue?.Substring(0, 1);
        }

        private static string GetDisplayName(object value)
        {
            var outString = string.Empty;
            var enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            var member = enumType.GetMember(enumValue)[0];
            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (!(attrs?.Length > 0)) return outString;
            outString = ((DisplayAttribute)attrs[0]).Name;
            if (((DisplayAttribute)attrs[0]).ResourceType != null)
                outString = ((DisplayAttribute)attrs[0]).GetName();

            return outString;
        }
    }
}
