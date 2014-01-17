namespace Narvalo.Web
{
    using System.Net;
    using System.Text;
    using System.Web;
    ////using Newtonsoft.Json;

    public static partial class HttpResponseExtensions
    {
        //// Cf. http://blogs.msdn.com/b/tmarq/archive/2009/06/25/correct-use-of-system-web-httpresponse-redirect.aspx
        ////protected void EndRequest(HttpContext context) {
        ////    context.ApplicationInstance.CompleteRequest();
        ////}

        ////public static void SendJson(this HttpResponse @this, object content)
        ////{
        ////    Require.Object(@this);

        ////    @this.ContentType = "application/json";
        ////    @this.ContentEncoding = Encoding.UTF8;
        ////    @this.Write(
        ////        JsonConvert.SerializeObject(
        ////            content,
        ////            new JsonSerializerSettings { Formatting = Formatting.None }));
        ////}
    }
}
