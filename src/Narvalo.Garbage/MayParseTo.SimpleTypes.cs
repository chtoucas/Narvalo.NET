namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;

    public static partial class MayParseTo
    {
        //// Boolean

        public static Maybe<bool> Boolean(string value)
        {
            return Boolean(value, BooleanStyles.Default);
        }

        public static Maybe<bool> Boolean(string value, BooleanStyles style)
        {
            return MayParseCore(
                value,
                (string val, out bool result) => TryParseTo.Boolean(val, style, out result));
        }

        //// Decimal

        public static Maybe<decimal> Decimal(string value)
        {
            return Decimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<decimal> Decimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out decimal result) =>
                {
                    return decimal.TryParse(val, style, provider, out result);
                });
        }

        //// Double

        public static Maybe<double> Double(string value)
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

        public static Maybe<double> Double(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out double result) => double.TryParse(val, style, provider, out result));
        }

        //// Int16

        public static Maybe<short> Int16(string value)
        {
            return Int16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<short> Int16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out short result) => short.TryParse(val, style, provider, out result));
        }

        //// Int32

        public static Maybe<int> Int32(string value)
        {
            return Int32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<int> Int32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out int result) => int.TryParse(val, style, provider, out result));
        }

        //// Int64

        public static Maybe<long> Int64(string value)
        {
            return Int64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<long> Int64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out long result) => long.TryParse(val, style, provider, out result));
        }

        //// Single

        public static Maybe<float> Single(string value)
        {
            return Single(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<float> Single(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out float result) => float.TryParse(val, style, provider, out result));
        }

        //// UInt16

        [CLSCompliant(false)]
        public static Maybe<ushort> UInt16(string value)
        {
            return UInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ushort> UInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out ushort result) => ushort.TryParse(val, style, provider, out result));
        }

        //// UInt32

        [CLSCompliant(false)]
        public static Maybe<uint> UInt32(string value)
        {
            return UInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<uint> UInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out uint result) => uint.TryParse(val, style, provider, out result));
        }

        //// UInt64

        [CLSCompliant(false)]
        public static Maybe<ulong> UInt64(string value)
        {
            return UInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ulong> UInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out ulong result) => ulong.TryParse(val, style, provider, out result));
        }
    }
}