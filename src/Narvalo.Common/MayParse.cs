namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static class MayParse
    {
        #region > Types élémentaires <

        public static Maybe<bool> ToBoolean(string value)
        {
            return ToBoolean(value, BooleanStyles.Default);
        }

        public static Maybe<bool> ToBoolean(string value, BooleanStyles style)
        {
            return MayParseHelper.Parse<bool>(
                value,
                (string val, out bool result) => BooleanUtility.TryParse(val, style, out result)
            );
        }

        public static Maybe<decimal> ToDecimal(string value)
        {
            return ToDecimal(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<decimal> ToDecimal(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<decimal>(value, (string val, out decimal result) =>
            {
                return Decimal.TryParse(val, style, provider, out result);
            });
        }

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
            return MayParseHelper.Parse<double>(
                value,
                (string val, out double result) => Double.TryParse(val, style, provider, out result)
            );
        }

        public static Maybe<short> ToInt16(string value)
        {
            return ToInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<short> ToInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<short>(
                value,
                (string val, out short result) => Int16.TryParse(val, style, provider, out result)
            );
        }

        public static Maybe<int> ToInt32(string value)
        {
            return ToInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<int> ToInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<int>(
                value,
                (string val, out int result) => Int32.TryParse(val, style, provider, out result)
            );
        }

        public static Maybe<long> ToInt64(string value)
        {
            return ToInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<long> ToInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<long>(
                value,
                (string val, out long result) => Int64.TryParse(val, style, provider, out result)
            );
        }

        public static Maybe<float> ToSingle(string value)
        {
            return ToSingle(value, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static Maybe<float> ToSingle(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<float>(
                value,
                (string val, out float result) => Single.TryParse(val, style, provider, out result)
            );
        }

        [CLSCompliant(false)]
        public static Maybe<ushort> ToUInt16(string value)
        {
            return ToUInt16(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ushort> ToUInt16(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<ushort>(
                value,
                (string val, out ushort result) => UInt16.TryParse(val, style, provider, out result)
            );
        }

        [CLSCompliant(false)]
        public static Maybe<uint> ToUInt32(string value)
        {
            return ToUInt32(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<uint> ToUInt32(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<uint>(
                value,
                (string val, out uint result) => UInt32.TryParse(val, style, provider, out result)
            );
        }

        [CLSCompliant(false)]
        public static Maybe<ulong> ToUInt64(string value)
        {
            return ToUInt64(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        [CLSCompliant(false)]
        public static Maybe<ulong> ToUInt64(string value, NumberStyles style, IFormatProvider provider)
        {
            return MayParseHelper.Parse<ulong>(
                value,
                (string val, out ulong result) => UInt64.TryParse(val, style, provider, out result)
            );
        }

        #endregion

        public static Maybe<DateTime> ToDateTime(string value)
        {
            return ToDateTime(value, "o");
        }

        public static Maybe<DateTime> ToDateTime(string value, string format)
        {
            return ToDateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static Maybe<DateTime> ToDateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            return MayParseHelper.Parse<DateTime>(
                value,
                (string val, out DateTime result) => DateTime.TryParseExact(val, format, provider, style, out result)
            );
        }

        public static Maybe<TEnum> ToEnum<TEnum>(string value) where TEnum : struct
        {
            return ToEnum<TEnum>(value, true /* ignoreCase */);
        }

        public static Maybe<TEnum> ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            Asserts.IsEnum(typeof(TEnum));

            return MayParseHelper.Parse<TEnum>(
                value,
                (string val, out TEnum result) => Enum.TryParse<TEnum>(val, ignoreCase, out result)
            );
        }

        public static Maybe<Uri> ToUri(string value, UriKind uriKind)
        {
            // NB: Uri.TryCreate accepte les chaînes vides.
            if (String.IsNullOrEmpty(value)) {
                return Maybe<Uri>.None;
            }

            return MayParseHelper.Parse<Uri>(
                value,
                (string val, out Uri result) => Uri.TryCreate(val, uriKind, out result)
            );
        }
    }
}