namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        public virtual bool IsReusable { get { return true; } }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected virtual bool TrySkipIisCustomErrors { get { return true; } }

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

        protected abstract void ProcessRequestCore(HttpContext context);

        protected virtual void HandleInvalidHttpMethod(HttpResponse response, string httpMethod)
        {
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write(Format.CurrentCulture(SR.HttpHandlerBase_InvalidHttpMethodFormat, httpMethod));
        }
    }
}
