namespace Narvalo.Web
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;

    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection nvc)
        {
            Require.Object(nvc);

            if (nvc.Count == 0) {
                return String.Empty;
            }

            // FIXME: "&amp;"
            return String.Join("&",
                Array.ConvertAll(
                    nvc.AllKeys,
                    key => String.Format(
                        CultureInfo.InvariantCulture,
                        "{0}={1}",
                        HttpUtility.UrlEncode(key),
                        HttpUtility.UrlEncode(nvc[key])
                    )
                )
            );

            //            return String.Join(
            //                "&amp;",
            //                query.AllKeys
            //                    .Where(key => !String.IsNullOrEmpty(query[key]))
            //                    .Select(
            //                        key => String.Format(
            //                            CultureInfo.InvariantCulture,
            //                            "{0}={1}",
            //                            HttpUtility.UrlEncode(key),
            //                            HttpUtility.UrlEncode(query[key]))
            //                        )
            //                    .ToArray()
            //            );
        }

        public static string ToQueryStringWithQuestionMark(this NameValueCollection nvc)
        {
            Require.Object(nvc);

            return nvc.Count > 0 ? "?" + nvc.ToQueryString() : String.Empty;
        }
    }
}
