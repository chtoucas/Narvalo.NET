// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Web.Properties;

    public static class UrlManip
    {
        // Also known as protocol-less URL.
        // <seealso cref="!:http://tools.ietf.org/html/rfc3986#section-4.2"/>
        public static string ToProtocolRelativeString(Uri uri)
        {
            Require.NotNull(uri, "uri");
            Contract.Ensures(Contract.Result<string>() != null);

            if (!uri.IsAbsoluteUri)
            {
                return uri.ToString();
            }

            var scheme = uri.Scheme;

            if (scheme == Uri.UriSchemeHttp)
            {
                return uri.ToString().Substring(5);
            }
            else if (scheme == Uri.UriSchemeHttps)
            {
                return uri.ToString().Substring(6);
            }
            else
            {
                throw new NotSupportedException(
                    Format.Resource(Strings_Web.UriExtensions_ProtocolRelativeUnsupportedScheme_Format, scheme));
            }
        }
    }
}
