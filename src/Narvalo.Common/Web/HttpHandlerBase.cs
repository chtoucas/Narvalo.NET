namespace Narvalo.Web
{
    using System.Net;
    using System.Web;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected virtual bool TrySkipIisCustomErrors { get { return true; } }

        protected abstract void ProcessRequestCore(HttpContext context);

        protected virtual void HandleInvalidHttpMethod(HttpResponse response, string httpMethod)
        {
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write("Invalid HTTP method: " + httpMethod);
        }

        #region IHttpHandler

        public virtual bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.TrySkipIisCustomErrors = TrySkipIisCustomErrors;

            // Validation de la méthode HTTP.
            var httpMethod = context.Request.HttpMethod;

            if (!AcceptedVerbs.Contains(httpMethod)) {
                HandleInvalidHttpMethod(context.Response, httpMethod);
                return;
            }

            ProcessRequestCore(context);
        }

        #endregion
    }
}
