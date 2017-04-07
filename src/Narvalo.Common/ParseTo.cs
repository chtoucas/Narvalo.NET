// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;

    using Narvalo.Applicative;
    using Narvalo.Internal;

    /// <summary>
    /// Provides parsing helpers.
    /// </summary>
    public static partial class ParseTo
    {
        public static bool? Boolean(string value) => Boolean(value, BooleanStyles.Default);

        public static bool? Boolean(string value, BooleanStyles style)
        {
            if (value == null) { return null; }

            var val = value.Trim();

            if (val.Length == 0)
            {
                return style.Contains(BooleanStyles.EmptyOrWhiteSpaceIsFalse) ? (bool?)false : null;
            }

            if (style.Contains(BooleanStyles.Literal))
            {
                // NB: Cette méthode n'est pas sensible à la casse de "value".
                if (System.Boolean.TryParse(val, out bool retval))
                {
                    return retval;
                }
            }

            if (style.Contains(BooleanStyles.ZeroOrOne) && (val == "0" || val == "1"))
            {
                return val == "1";
            }

            if (style.Contains(BooleanStyles.HtmlInput) && value == "on")
            {
                return true;
            }

            return null;
        }

        public static decimal? Decimal(string value)
            => Decimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);

        public static decimal? Decimal(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<decimal> parser = (string v, out decimal result)
                => System.Decimal.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

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
            TryParser<double> parser = (string v, out double result)
                => System.Double.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static short? Int16(string value)
            => Int16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        public static short? Int16(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<short> parser = (string v, out short result)
                => System.Int16.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static int? Int32(string value)
            => Int32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        public static int? Int32(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<int> parser = (string v, out int result)
                => System.Int32.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static long? Int64(string value)
            => Int64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        public static long? Int64(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<long> parser = (string v, out long result)
                => System.Int64.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static float? Single(string value)
            => Single(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);

        public static float? Single(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<float> parser = (string v, out float result)
                => System.Single.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static sbyte? SByte(string value)
            => SByte(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);

        [CLSCompliant(false)]
        public static sbyte? SByte(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<sbyte> parser = (string v, out sbyte result)
                => System.SByte.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static byte? Byte(string value)
            => Byte(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);

        [CLSCompliant(false)]
        public static byte? Byte(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<byte> parser = (string v, out byte result)
                => System.Byte.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static ushort? UInt16(string value)
            => UInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        [CLSCompliant(false)]
        public static ushort? UInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<ushort> parser = (string v, out ushort result)
                => System.UInt16.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static uint? UInt32(string value)
            => UInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        [CLSCompliant(false)]
        public static uint? UInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<uint> parser = (string v, out uint result)
                => System.UInt32.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static ulong? UInt64(string value)
            => UInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);

        [CLSCompliant(false)]
        public static ulong? UInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<ulong> parser = (string v, out ulong result)
                => System.UInt64.TryParse(v, style, provider, out result);

            return parser.NullInvoke(value);
        }
    }

    // Provides parsers for value types that are not simple types.
    public static partial class ParseTo
    {
        public static TEnum? Enum<TEnum>(string value) where TEnum : struct
            => Enum<TEnum>(value, ignoreCase: true);

        // TODO: Explain that this method exhibits the same behaviour as Enum.TryParse,
        // in the sense that parsing any literal integer value will succeed even if
        // it is not a valid enumeration value.
        // See http://stackoverflow.com/questions/2191037/why-can-i-parse-invalid-values-to-an-enum-in-net
        public static TEnum? Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            TryParser<TEnum> parser = (string v, out TEnum result)
                => System.Enum.TryParse<TEnum>(v, ignoreCase, out result);

            return parser.NullInvoke(value);
        }

        public static DateTime? DateTime(string value) => DateTime(value, "o");

        public static DateTime? DateTime(string value, string format)
            => DateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);

        public static DateTime? DateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            TryParser<DateTime> parser = (string v, out DateTime result)
                => System.DateTime.TryParseExact(v, format, provider, style, out result);

            return parser.NullInvoke(value);
        }
    }

    // Provides parsers for reference types.
    public static partial class ParseTo
    {
        public static Maybe<Uri> Uri(string value, UriKind uriKind)
        {
            // REVIEW: Uri.TryCreate accepts empty strings.
            if (String.IsNullOrWhiteSpace(value))
            {
                return Maybe<Uri>.None;
            }

            TryParser<Uri> parser = (string v, out Uri result)
                => System.Uri.TryCreate(v, uriKind, out result);

            return parser.MayInvoke(value);
        }
    }
}
