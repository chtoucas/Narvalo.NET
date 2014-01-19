namespace Narvalo
{
    using System;
    using System.Globalization;

    public static partial class Create
    {
        //// Boolean

        public static bool? Boolean(string value)
        {
            return Boolean(value, BooleanStyles.Default);
        }

        public static bool? Boolean(string value, BooleanStyles style)
        {
            return CreateCore(
                value,
                (string val, out bool result) => TryParse.ToBoolean(val, style, out result));
        }

        //// Decimal

        public static decimal? Decimal(string value)
        {
            return Decimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal? Decimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out decimal result) =>
                {
                    return decimal.TryParse(val, style, provider, out result);
                });
        }

        //// Double

        public static double? Double(string value)
        {
            var style
                = NumberStyles.AllowLeadingWhite
                | NumberStyles.AllowTrailingWhite
                | NumberStyles.AllowLeadingSign
                | NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowThousands
                | NumberStyles.AllowExponent;

            return Double(value, style, NumberFormatInfo.CurrentInfo);
        }

        public static double? Double(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out double result) => double.TryParse(val, style, provider, out result));
        }

        //// Int16

        public static short? Int16(string value)
        {
            return Int16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short? Int16(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out short result) => short.TryParse(val, style, provider, out result));
        }

        //// Int32

        public static int? Int32(string value)
        {
            return Int32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int? Int32(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out int result) => int.TryParse(val, style, provider, out result));
        }

        //// Int64

        public static long? Int64(string value)
        {
            return Int64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long? Int64(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out long result) => long.TryParse(val, style, provider, out result));
        }

        //// Single

        public static float? Single(string value)
        {
            return Single(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static float? Single(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out float result) => float.TryParse(val, style, provider, out result));
        }

        //// UInt16

        [CLSCompliant(false)]
        public static ushort? UInt16(string value)
        {
            return UInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ushort? UInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out ushort result) => ushort.TryParse(val, style, provider, out result));
        }

        //// UInt32

        [CLSCompliant(false)]
        public static uint? UInt32(string value)
        {
            return UInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static uint? UInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out uint result) => uint.TryParse(val, style, provider, out result));
        }

        //// UInt64

        [CLSCompliant(false)]
        public static ulong? UInt64(string value)
        {
            return UInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static ulong? UInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return CreateCore(
                value,
                (string val, out ulong result) => ulong.TryParse(val, style, provider, out result));
        }
    }
}
