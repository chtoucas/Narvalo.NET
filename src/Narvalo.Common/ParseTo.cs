// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Globalization;
    using System.Net;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static partial class ParseTo
    {
        //// Enum

        public static TEnum? Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, ignoreCase: true);
        }

        public static TEnum? Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) {
                throw new InvalidOperationException(
                    Format.CurrentCulture(Strings_Common.TypeIsNotEnumFormat, type.FullName ?? "Unknown type name"));
            }

            TryParser<TEnum> parser = (string _, out TEnum result) => System.Enum.TryParse<TEnum>(_, ignoreCase, out result);

            return parser.NullInvoke(value);
        }

        //// DateTime

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

        //// Uri

        public static Maybe<Uri> Uri(string value, UriKind uriKind)
        {
            // REVIEW: Uri.TryCreate accepts empty strings.
            if (String.IsNullOrWhiteSpace(value)) {
                return Maybe<Uri>.None;
            }

            TryParser<Uri> parser = (string _, out Uri result) => System.Uri.TryCreate(_, uriKind, out result);

            return parser.MayInvoke(value);
        }

        //// IPAddress

        public static Maybe<IPAddress> IPAddress(string value)
        {
            TryParser<IPAddress> parser = (string _, out IPAddress result) => System.Net.IPAddress.TryParse(_, out result);

            return parser.MayInvoke(value);
        }
    }
}
