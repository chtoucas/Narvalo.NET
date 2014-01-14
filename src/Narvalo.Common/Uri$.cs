namespace Narvalo
{
    using System;

    public static class UriExtensions
    {
        public static string ToProtocolLessString(this Uri uri)
        {
            if (uri.Scheme == Uri.UriSchemeHttp) {
                var uriString = uri.ToString();
                return uri.IsAbsoluteUri ? uriString.Replace("http:", String.Empty) : uriString;
            }
            else if (uri.Scheme == Uri.UriSchemeHttps) {
                var uriString = uri.ToString();
                return uri.IsAbsoluteUri ? uriString.Replace("https:", String.Empty) : uriString;
            }
            else {
                throw Failure.NotSupported(SR.Uri_ProtocolLessUnsupportedSchemeFormat, uri.Scheme);
            }
        }
    }
}
