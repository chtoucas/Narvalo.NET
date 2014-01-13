namespace Narvalo.Web
{
    using System;
    using System.IO.Compression;
    using System.Web;

    public sealed class HttpCompressionModule : IHttpModule
    {
        #region IHttpModule

        void IHttpModule.Dispose()
        {
            ;
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.PostAcquireRequestState += new EventHandler(context_PostAcquireRequestState);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication context = sender as HttpApplication;
            context.PostAcquireRequestState -= new EventHandler(context_PostAcquireRequestState);
            context.EndRequest -= new EventHandler(context_EndRequest);
        }

        void context_PostAcquireRequestState(object sender, EventArgs e)
        {
            RegisterCompressFilter();
        }

        private static void RegisterCompressFilter()
        {
            HttpContext context = HttpContext.Current;

            if (context.Handler is StaticFileHandler
                || context.Handler is DefaultHttpHandler) {
                return;
            }

            HttpRequest request = context.Request;

            string acceptEncoding = request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(acceptEncoding)) {
                return;
            }

            acceptEncoding = acceptEncoding.ToUpperInvariant();

            HttpResponse response = HttpContext.Current.Response;

            if (acceptEncoding.Contains("GZIP")) {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (acceptEncoding.Contains("DEFLATE")) {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }
        }

        #endregion
    }
}
