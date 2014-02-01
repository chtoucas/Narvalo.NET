namespace Narvalo.Web
{
    using System.Net;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Provides extension methods for <see cref="System.Web.HttpResponse"/>.
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
