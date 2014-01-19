namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;

    public static partial class MayParse
    {
        //// Boolean

        public static Maybe<bool> ToBoolean(string value)
        {
            return ToBoolean(value, BooleanStyles.Default);
        }

        public static Maybe<bool> ToBoolean(string value, BooleanStyles style)
        {
            return MayParseCore(
                value,
                (string val, out bool result) => TryParse.ToBoolean(val, style, out result));
        }

        //// Decimal

        public static Maybe<decimal> ToDecimal(string value)
        {
            return ToDecimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<decimal> ToDecimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out decimal result) =>
                {
                    return Decimal.TryParse(val, style, provider, out result);
                });
        }

        //// Double

        public static Maybe<double> ToDouble(string value)
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

        public static Maybe<double> ToDouble(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out double result) => Double.TryParse(val, style, provider, out result));
        }

        //// Int16

        public static Maybe<short> ToInt16(string value)
        {
            return ToInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<short> ToInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out short result) => Int16.TryParse(val, style, provider, out result));
        }

        //// Int32

        public static Maybe<int> ToInt32(string value)
        {
            return ToInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<int> ToInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out int result) => Int32.TryParse(val, style, provider, out result));
        }

        //// Int64

        public static Maybe<long> ToInt64(string value)
        {
            return ToInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<long> ToInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out long result) => Int64.TryParse(val, style, provider, out result));
        }

        //// Single

        public static Maybe<float> ToSingle(string value)
        {
            return ToSingle(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<float> ToSingle(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out float result) => Single.TryParse(val, style, provider, out result));
        }

        //// UInt16

        [CLSCompliant(false)]
        public static Maybe<ushort> ToUInt16(string value)
        {
            return ToUInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ushort> ToUInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out ushort result) => UInt16.TryParse(val, style, provider, out result));
        }

        //// UInt32

        [CLSCompliant(false)]
        public static Maybe<uint> ToUInt32(string value)
        {
            return ToUInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<uint> ToUInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out uint result) => UInt32.TryParse(val, style, provider, out result));
        }

        //// UInt64

        [CLSCompliant(false)]
        public static Maybe<ulong> ToUInt64(string value)
        {
            return ToUInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ulong> ToUInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseCore(
                value,
                (string val, out ulong result) => UInt64.TryParse(val, style, provider, out result));
        }
    }
}