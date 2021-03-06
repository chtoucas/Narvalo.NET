﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;

    using Narvalo.Web.Properties;

    public static class UrlManip
    {
        public static string ToProtocolRelativeString(Uri uri)
        {
            Require.NotNull(uri, nameof(uri));

            if (!uri.IsAbsoluteUri)
            {
                return uri.ToString();
            }

            var scheme = uri.Scheme;

            if (scheme == Uri.UriSchemeHttp)
            {
                var uriString = uri.ToString();

                return uriString.Substring(5);
            }
            else if (scheme == Uri.UriSchemeHttps)
            {
                var uriString = uri.ToString();

                return uriString.Substring(6);
            }
            else
            {
                throw new NotSupportedException(
                    Format.Current(Strings.UriExtensions_ProtocolRelativeUnsupportedScheme_Format, scheme));
            }
        }
    }
}
