// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Net;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static class MayParseTo
    {
        public static Maybe<IPAddress> IPAddress(string value)
        {
            return MayParseCore_(
                value,
                (string val, out IPAddress result) => System.Net.IPAddress.TryParse(val, out result));
        }

        public static Maybe<Uri> Uri(string value, UriKind uriKind)
        {
            // REVIEW: Uri.TryCreate accepts empty strings.
            if (String.IsNullOrWhiteSpace(value)) {
                return Maybe<Uri>.None;
            }

            return MayParseCore_(
                value,
                (string val, out Uri result) => System.Uri.TryCreate(val, uriKind, out result));
        }

        //// MayParseCore

        static Maybe<T> MayParseCore_<T>(string value, TryParse<T> tryParser) where T : class
        {
            if (value == null) { return Maybe<T>.None; }

            T result;
            return tryParser(value, out result) ? Maybe.Create(result) : Maybe<T>.None;
        }
    }
}