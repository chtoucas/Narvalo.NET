namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected abstract void ProcessRequestCore(HttpContext context);

        protected virtual void ProcessInvalidHttpMethod(HttpResponse response)
        {
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
        }

        #region IHttpHandler

        public virtual bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            // Validation de la méthode HTTP.
            if (!AcceptedVerbs.Contains(context.Request.HttpMethod)) {
                ProcessInvalidHttpMethod(context.Response);
                return;
            }

            ProcessRequestCore(context);
        }

        #endregion
    }
}
