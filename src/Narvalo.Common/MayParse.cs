namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static partial class MayParse
    {
        //// Enum

        public static Maybe<TEnum> ToEnum<TEnum>(string value) where TEnum : struct
        {
            return ToEnum<TEnum>(value, true /* ignoreCase */);
        }

        public static Maybe<TEnum> ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return MayParseCore(
                value,
                (string val, out TEnum result) => Enum.TryParse<TEnum>(val, ignoreCase, out result));
        }

        //// DateTime

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
            return MayParseCore(
                value,
                (string val, out DateTime result) => DateTime.TryParseExact(val, format, provider, style, out result));
        }

        //// MayParseCore

        internal static Maybe<T> MayParseCore<T>(string value, TryParse<T> fun) where T : struct
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return fun(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}