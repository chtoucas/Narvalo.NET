// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static class UriExtensions
    {
        public static string ToProtocolLessString(this Uri @this)
        {
            Require.Object(@this);

            if (!@this.IsAbsoluteUri) {
                return @this.ToString();
            }

            var scheme = @this.Scheme;

            if (scheme == Uri.UriSchemeHttp) {
                return @this.ToString().Replace("http:", String.Empty);
            }
            else if (scheme == Uri.UriSchemeHttps) {
                return @this.ToString().Replace("https:", String.Empty);
            }
            else {
                throw new NotSupportedException(
                    Format.CurrentCulture(SRCommon.Uri_ProtocolLessUnsupportedSchemeFormat, scheme));
            }
        }
    }
}
