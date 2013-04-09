namespace Narvalo.Web
{
    using System.Web;
    using System.Web.Mvc;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected abstract void ProcessRequestCore(HttpContext context);

        #region IHttpHandler

        public virtual bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            // Validation de la méthode HTTP.
            if (!AcceptedVerbs.Contains(context.Request.HttpMethod)) {
                // XXX
                return;
            }

            ProcessRequestCore(context);
        }

        #endregion
    }
}
