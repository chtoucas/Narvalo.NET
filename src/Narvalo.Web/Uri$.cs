// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Diagnostics.Contracts;

    public static class UriExtensions
    {
        public static string ToProtocolLessString(this Uri @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<string>() != null);

            if (!@this.IsAbsoluteUri)
            {
                return @this.ToString();
            }

            var scheme = @this.Scheme;

            if (scheme == Uri.UriSchemeHttp)
            {
                return @this.ToString().Replace("http:", String.Empty);
            }
            else if (scheme == Uri.UriSchemeHttps)
            {
                return @this.ToString().Replace("https:", String.Empty);
            }
            else
            {
                throw new NotSupportedException(
                    Format.CurrentCulture(Strings_Web.Uri_ProtocolLessUnsupportedSchemeFormat, scheme));
            }
        }
    }
}
