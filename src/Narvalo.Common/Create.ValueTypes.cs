namespace Narvalo
{
    using System;
    using System.Globalization;

    public static partial class Create
    {
        //// Enum

        public static TEnum? Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, true /* ignoreCase */);
        }

        public static TEnum? Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return CreateCore(
                value,
                (string val, out TEnum result) => System.Enum.TryParse<TEnum>(val, ignoreCase, out result));
        }

        //// DateTime

        public static DateTime? DateTime(string value)
        {
            return DateTime(value, "o");
        }

        public static DateTime? DateTime(string value, string format)
        {
            return DateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime? DateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            return CreateCore(
                value,
                (string val, out DateTime result) => System.DateTime.TryParseExact(val, format, provider, style, out result));
        }
    }
}
