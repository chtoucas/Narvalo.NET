// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Text;
    using System.Web;

    using Narvalo.Properties;

    public static class HttpRequestBaseExtensions
    {
        public static NameValueCollection DecodeKeyValuePost(this HttpRequestBase @this)
        {
            Require.Object(@this);

            return HttpUtility.ParseQueryString(SlurpBody_(@this));
        }

        //        public static NameValueCollection DecodeJsonAjaxRequest(HttpRequestBase request) {
        //            return HttpUtility.ParseQueryString(ReadBody(request));
        //        }

        static string SlurpBody_(HttpRequestBase request)
        {
            Stream bodyStream = request.InputStream;

            // FIXME: Int32.MaxValue ?
            if (bodyStream.Length > Int64.MaxValue) {
                throw new ArgumentException(Strings_Brouillons.HttpRequestBase_InputStreamTooLarge, "request");
            }

            int length = Convert.ToInt32(bodyStream.Length);
            byte[] byteArray = new byte[length];

            bodyStream.Read(byteArray, 0, length);

            var sb = new StringBuilder();

            for (int i = 0; i < length; i++) {
                sb.Append(Convert.ToChar(byteArray[i]));
            }

            return sb.ToString();
        }
    }
}
