namespace Narvalo
{
    using System;

    public static class UriExtensions
    {
        public static string ToProtocolLessString(this Uri @this)
        {
            Require.Object(@this);

            var scheme = @this.Scheme;

            if (!@this.IsAbsoluteUri) {
                return @this.ToString();
            }
            else if (scheme == Uri.UriSchemeHttp) {
                return @this.ToString().Replace("http:", String.Empty);
            }
            else if (scheme == Uri.UriSchemeHttps) {
                return @this.ToString().Replace("https:", String.Empty);
            }
            else {
                throw new NotSupportedException(
                    Format.CurrentCulture(SR.Uri_ProtocolLessUnsupportedSchemeFormat, scheme));
            }
        }
    }
}
