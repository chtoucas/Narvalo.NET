// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static partial class MayParseTo
    {
        //// Enum

        public static Maybe<TEnum> Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, true /* ignoreCase */);
        }

        public static Maybe<TEnum> Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return MayParseCore(
                value,
                (string val, out TEnum result) => System.Enum.TryParse<TEnum>(val, ignoreCase, out result));
        }

        //// DateTime

        public static Maybe<DateTime> DateTime(string value)
        {
            return DateTime(value, "o");
        }

        public static Maybe<DateTime> DateTime(string value, string format)
        {
            return DateTime(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        public static Maybe<DateTime> DateTime(
            string value,
            string format,
            IFormatProvider provider,
            DateTimeStyles style)
        {
            return MayParseCore(
                value,
                (string val, out DateTime result) => System.DateTime.TryParseExact(val, format, provider, style, out result));
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
