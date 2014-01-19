namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Internal;

    public static class ParseValue
    {
        //// Boolean

        public static bool? ToBoolean(string value)
        {
            return ToBoolean(value, BooleanStyles.Default);
        }

        public static bool? ToBoolean(string value, BooleanStyles style)
        {
            return ParseValueCore<bool>(
                value,
                (string val, out bool result) => TryParse.ToBoolean(val, style, out result));
        }

        //// Decimal

        public static decimal? ToDecimal(string value)
        {
            return ToDecimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal? ToDecimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<decimal>(
                value,
                (string val, out decimal result) =>
                {
                    return Decimal.TryParse(val, style, provider, out result);
                });
        }

        //// Double

        public static double? ToDouble(string value)
        {
            var style
                = NumberStyles.AllowLeadingWhite
                | NumberStyles.AllowTrailingWhite
                | NumberStyles.AllowLeadingSign
                | NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowThousands
                | NumberStyles.AllowExponent;

            return ToDouble(value, style, NumberFormatInfo.CurrentInfo);
        }

        public static double? ToDouble(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<double>(
                value,
                (string val, out double result) => Double.TryParse(val, style, provider, out result));
        }

        //// Int16

        public static short? ToInt16(string value)
        {
            return ToInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short? ToInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<short>(
                value,
                (string val, out short result) => Int16.TryParse(val, style, provider, out result));
        }

        //// Int32

        public static int? ToInt32(string value)
        {
            return ToInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int? ToInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<int>(
                value,
                (string val, out int result) => Int32.TryParse(val, style, provider, out result));
        }

        //// Int64

        public static long? ToInt64(string value)
        {
            return ToInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long? ToInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<long>(
                value,
                (string val, out long result) => Int64.TryParse(val, style, provider, out result));
        }

        //// Single

        public static float? ToSingle(string value)
        {
            return ToSingle(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static float? ToSingle(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<float>(
                value,
                (string val, out float result) => Single.TryParse(val, style, provider, out result));
        }

        //// UInt16

        [CLSCompliant(false)]
        public static ushort? ToUInt16(string value)
        {
            return ToUInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort? ToUInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<ushort>(
                value,
                (string val, out ushort result) => UInt16.TryParse(val, style, provider, out result));
        }

        //// UInt32

        [CLSCompliant(false)]
        public static uint? ToUInt32(string value)
        {
            return ToUInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static uint? ToUInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<uint>(
                value,
                (string val, out uint result) => UInt32.TryParse(val, style, provider, out result));
        }

        //// UInt64

        [CLSCompliant(false)]
        public static ulong? ToUInt64(string value)
        {
            return ToUInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ulong? ToUInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseValueCore<ulong>(
                value,
                (string val, out ulong result) => UInt64.TryParse(val, style, provider, out result));
        }

        //// DateTime

        public static DateTime? ToDateTime(string value)
        {
            return ToDateTime(value, "o");
        }

        public static DateTime? ToDateTime(string value, string format)
        {
            return ToDateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static DateTime? ToDateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            return ParseValueCore<DateTime>(
                value,
                (string val, out DateTime result) => DateTime.TryParseExact(val, format, provider, style, out result));
        }

        //// Enum

        public static TEnum? ToEnum<TEnum>(string value) where TEnum : struct
        {
            return ToEnum<TEnum>(value, true /* ignoreCase */);
        }

        public static TEnum? ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return ParseValueCore<TEnum>(
                value,
                (string val, out TEnum result) => Enum.TryParse<TEnum>(val, ignoreCase, out result));
        }

        internal static T? ParseValueCore<T>(string value, TryParse<T> fun) where T : struct
        {
            if (value == null) { return null; }

            T result;
            return fun(value, out result) ? result : (T?)null;
        }
    }
}
