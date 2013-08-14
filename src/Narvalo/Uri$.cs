namespace Narvalo
{
    using System;

    public static class UriExtensions
    {
        public static string ToProtocolLessString(this Uri uri)
        {
            if (!uri.IsAbsoluteUri) {
                return uri.ToString();
            }
            else if (uri.Scheme == Uri.UriSchemeHttp) {
                return uri.ToString().Replace("http:", String.Empty);
            }
            else if (uri.Scheme == Uri.UriSchemeHttps) {
                return uri.ToString().Replace("https:", String.Empty);
            }
            else {
                return uri.ToString();
            }
        }
    }
}
