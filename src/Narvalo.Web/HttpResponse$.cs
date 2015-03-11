// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Net;
    using System.Web;

    /// <summary>
    /// Provides extension methods for <see cref="HttpResponse"/>.
    /// </summary>
    public static partial class HttpResponseExtensions
    {
        public static void SendPlainText(this HttpResponse @this, string content)
        {
            Require.Object(@this);

            @this.ContentType = "text/plain";
            @this.Write(content);
        }

        public static void SetStatusCode(this HttpResponse @this, HttpStatusCode statusCode)
        {
            Require.Object(@this);

            @this.StatusCode = (int)statusCode;
        }
    }
}
