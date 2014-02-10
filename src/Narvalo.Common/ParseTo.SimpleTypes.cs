// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;

    public static partial class ParseTo
    {
        //// Boolean

        public static bool? Boolean(string value)
        {
            return Boolean(value, BooleanStyles.Default);
        }

        public static bool? Boolean(string value, BooleanStyles style)
        {
            if (value == null) { return null; }

            var val = value.Trim();

            if (val.Length == 0) {
                return style.HasFlag(BooleanStyles.EmptyIsFalse) ? (bool?)false : null;
            }

            if (style.HasFlag(BooleanStyles.Literal)) {
                bool result;

                // NB: Cette méthode n'est pas sensible à la casse de "value".
                if (System.Boolean.TryParse(val, out result)) {
                    return result;
                }
            }

            if (style.HasFlag(BooleanStyles.OneOrZero) && (val == "0" || val == "1")) {
                return val == "1";
            }

            if (style.HasFlag(BooleanStyles.HtmlInput) && value == "on") {
                return true;
            }

            return null;
        }

        //// Decimal

        public static decimal? Decimal(string value)
        {
            return Decimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal? Decimal(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<decimal> parser = (string _, out decimal result) => decimal.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
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
            TryParser<double> parser = (string _, out double result) => double.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        //// Int16

        public static short? Int16(string value)
        {
            return Int16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short? Int16(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<short> parser = (string _, out short result) => short.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        //// Int32

        public static int? Int32(string value)
        {
            return Int32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int? Int32(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<int> parser = (string _, out int result) => int.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        //// Int64

        public static long? Int64(string value)
        {
            return Int64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long? Int64(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<long> parser = (string _, out long result) => long.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        //// Single

        public static float? Single(string value)
        {
            return Single(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static float? Single(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<float> parser = (string _, out float result) => float.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
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
            TryParser<ushort> parser = (string _, out ushort result) => ushort.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
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
            TryParser<uint> parser = (string _, out uint result) => uint.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
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
            TryParser<ulong> parser = (string _, out ulong result) => ulong.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }
    }
}
