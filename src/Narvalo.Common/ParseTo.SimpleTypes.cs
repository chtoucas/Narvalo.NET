namespace Narvalo
{
    using System;
    using System.Globalization;

    public static partial class ParseTo
    {
        //// Boolean

        public static bool? NullableBoolean(string value)
        {
            return NullableBoolean(value, BooleanStyles.Default);
        }

        public static bool? NullableBoolean(string value, BooleanStyles style)
        {
            return ParseCore(
                value,
                (string val, out bool result) => TryParseTo.Boolean(val, style, out result));
        }

        //// Decimal

        public static decimal? NullableDecimal(string value)
        {
            return NullableDecimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal? NullableDecimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out decimal result) =>
                {
                    return decimal.TryParse(val, style, provider, out result);
                });
        }

        //// Double

        public static double? NullableDouble(string value)
        {
            var style
                = NumberStyles.AllowLeadingWhite
                | NumberStyles.AllowTrailingWhite
                | NumberStyles.AllowLeadingSign
                | NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowThousands
                | NumberStyles.AllowExponent;

            return NullableDouble(value, style, NumberFormatInfo.CurrentInfo);
        }

        public static double? NullableDouble(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out double result) => double.TryParse(val, style, provider, out result));
        }

        //// Int16

        public static short? NullableInt16(string value)
        {
            return NullableInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short? NullableInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out short result) => short.TryParse(val, style, provider, out result));
        }

        //// Int32

        public static int? NullableInt32(string value)
        {
            return NullableInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int? NullableInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out int result) => int.TryParse(val, style, provider, out result));
        }

        //// Int64

        public static long? NullableInt64(string value)
        {
            return NullableInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long? NullableInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out long result) => long.TryParse(val, style, provider, out result));
        }

        //// Single

        public static float? NullableSingle(string value)
        {
            return NullableSingle(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static float? NullableSingle(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out float result) => float.TryParse(val, style, provider, out result));
        }

        //// UInt16

        [CLSCompliant(false)]
        public static ushort? NullableUInt16(string value)
        {
            return NullableUInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort? NullableUInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out ushort result) => ushort.TryParse(val, style, provider, out result));
        }

        //// UInt32

        [CLSCompliant(false)]
        public static uint? NullableUInt32(string value)
        {
            return NullableUInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static uint? NullableUInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out uint result) => uint.TryParse(val, style, provider, out result));
        }

        //// UInt64

        [CLSCompliant(false)]
        public static ulong? NullableUInt64(string value)
        {
            return NullableUInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ulong? NullableUInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return ParseCore(
                value,
                (string val, out ulong result) => ulong.TryParse(val, style, provider, out result));
        }
    }
}
