namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Internal;

    public static partial class ParseTo
    {
        //// Enum

        public static TEnum Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, ignoreCase: false);
        }

        public static TEnum Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

        public static TEnum? NullableEnum<TEnum>(string value) where TEnum : struct
        {
            return NullableEnum<TEnum>(value, ignoreCase: true);
        }

        public static TEnum? NullableEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return ParseCore(
                value,
                (string val, out TEnum result) => System.Enum.TryParse<TEnum>(val, ignoreCase, out result));
        }

        //// DateTime

        public static DateTime? NullableDateTime(string value)
        {
            return NullableDateTime(value, "o");
        }

        public static DateTime? NullableDateTime(string value, string format)
        {
            return NullableDateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime? NullableDateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            return ParseCore(
                value,
                (string val, out DateTime result) => System.DateTime.TryParseExact(val, format, provider, style, out result));
        }

        //// ParseCore

        internal static T? ParseCore<T>(string value, TryParse<T> fun) where T : struct
        {
            if (value == null) { return null; }

            T result;
            return fun(value, out result) ? result : (T?)null;
        }
    }
}
