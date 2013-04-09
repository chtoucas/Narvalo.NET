namespace Narvalo.Web
{
    using System.Net;
    using System.Text;
    using System.Web;
    using Narvalo.Diagnostics;
    using Newtonsoft.Json;

    public static class HttpResponseExtensions
    {
        //// Cf. http://blogs.msdn.com/b/tmarq/archive/2009/06/25/correct-use-of-system-web-httpresponse-redirect.aspx
        //protected void EndRequest(HttpContext context) {
        //    context.ApplicationInstance.CompleteRequest();
        //}

        #region Content-type shortcuts

        public static void SendJson(this HttpResponse response, object content)
        {
            Requires.NotNull(response);

            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.Write(
                JsonConvert.SerializeObject(
                    content,
                    new JsonSerializerSettings { Formatting = Formatting.None }));
        }

        public static void SendPlainText(this HttpResponse response, string content)
        {
            Requires.NotNull(response);

            response.ContentType = "text/plain";
            response.Write(content);
        }

        #endregion

        public static void SetStatusCode(this HttpResponse response, HttpStatusCode statusCode)
        {
            Requires.NotNull(response);

            response.StatusCode = (int)statusCode;
        }
    }
}
