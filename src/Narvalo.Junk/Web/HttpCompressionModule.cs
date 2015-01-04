// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.IO.Compression;
    using System.Web;

    public sealed class HttpCompressionModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            context.PostAcquireRequestState += OnPostAcquireRequestState_;
            context.EndRequest += OnEndRequest_;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        void OnEndRequest_(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;
            application.PostAcquireRequestState -= OnPostAcquireRequestState_;
            application.EndRequest -= OnEndRequest_;
        }

        void OnPostAcquireRequestState_(object sender, EventArgs e)
        {
            var application = sender as HttpApplication;

            RegisterCompressFilter_(application.Context);
        }

        static void RegisterCompressFilter_(HttpContext context)
        {
            if (context.Handler is StaticFileHandler
                || context.Handler is DefaultHttpHandler) {
                return;
            }

            var request = context.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding)) {
                return;
            }

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            HttpResponse response = context.Response;

            if (acceptEncoding.Contains("GZIP")) {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE")) {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }
    }
}
