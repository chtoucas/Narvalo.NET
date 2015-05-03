// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;

    using Narvalo.Fx;
    using Narvalo.Internal;

    /// <summary>
    /// Provides parsing helpers.
    /// </summary>
    public static partial class ParseTo
    {
        public static bool? Boolean(string value)
        {
            return Boolean(value, BooleanStyles.Default);
        }

        public static bool? Boolean(string value, BooleanStyles style)
        {
            if (value == null) { return null; }

            var val = value.Trim();

            if (val.Length == 0)
            {
                return style.Contains(BooleanStyles.EmptyIsFalse) ? (bool?)false : null;
            }

            if (style.Contains(BooleanStyles.Literal))
            {
                bool retval;

                // NB: Cette méthode n'est pas sensible à la casse de "value".
                if (System.Boolean.TryParse(val, out retval))
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
        {
            return Decimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal? Decimal(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<decimal> parser =
                (string _, out decimal result) => decimal.TryParse(_, style, provider, out result);

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
            TryParser<double> parser = (string _, out double result) => double.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static short? Int16(string value)
        {
            return Int16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static short? Int16(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<short> parser = (string _, out short result) => short.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static int? Int32(string value)
        {
            return Int32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static int? Int32(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<int> parser = (string _, out int result) => int.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static long? Int64(string value)
        {
            return Int64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static long? Int64(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<long> parser = (string _, out long result) => long.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        public static float? Single(string value)
        {
            return Single(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static float? Single(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<float> parser = (string _, out float result) => float.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static sbyte? SByte(string value)
        {
            return SByte(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static sbyte? SByte(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<sbyte> parser = (string _, out sbyte result) => sbyte.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

        [CLSCompliant(false)]
        public static byte? Byte(string value)
        {
            return Byte(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static byte? Byte(string value, NumberStyles style, IFormatProvider provider)
        {
            TryParser<byte> parser = (string _, out byte result) => byte.TryParse(_, style, provider, out result);

            return parser.NullInvoke(value);
        }

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

        #region Implements parsers for value types that are not simple types

        public static TEnum? Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, ignoreCase: true);
        }

        public static TEnum? Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            TryParser<TEnum> parser =
                (string _, out TEnum result) => System.Enum.TryParse<TEnum>(_, ignoreCase, out result);

            return parser.NullInvoke(value);
        }

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
            TryParser<DateTime> parser = (string _, out DateTime result)
                => System.DateTime.TryParseExact(_, format, provider, style, out result);

            return parser.NullInvoke(value);
        }

        #endregion

        #region Implements parsers for reference types

        public static Maybe<Uri> Uri(string value, UriKind uriKind)
        {
            // REVIEW: Uri.TryCreate accepts empty strings.
            if (String.IsNullOrWhiteSpace(value))
            {
                return Maybe<Uri>.None;
            }

            TryParser<Uri> parser = (string _, out Uri result) => System.Uri.TryCreate(_, uriKind, out result);

            return parser.MayInvoke(value);
        }

        ////public static Maybe<IPAddress> IPAddress(string value)
        ////{
        ////    Contract.Ensures(Contract.Result<Maybe<IPAddress>>() != null);

        ////    TryParser<IPAddress> parser = (string _, out IPAddress result) => System.Net.IPAddress.TryParse(_, out result);

        ////    return parser.MayInvoke(value);
        ////}

        #endregion
    }
}
