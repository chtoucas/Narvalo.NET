namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Narvalo.Linq;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        public virtual bool IsReusable { get { return false; } }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected virtual bool TrySkipIisCustomErrors { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.TrySkipIisCustomErrors = TrySkipIisCustomErrors;

            if (ValidateHttpMethod(context.Request)) {
                ProcessRequestCore(context);
            }
            else {
                OnInvalidHttpMethod(context);
            }
        }

        protected abstract void ProcessRequestCore(HttpContext context);

        protected virtual bool ValidateHttpMethod(HttpRequest request)
        {
            DebugCheck.NotNull(request);

            return (from _ in ParseTo.Enum<HttpVerbs>(request.HttpMethod) select AcceptedVerbs.HasFlag(_)) ?? false;
        }

        protected virtual void OnInvalidHttpMethod(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var response = context.Response;

            // TODO: Indiquer les méthodes autorisées dans la réponse.
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write(Format.CurrentCulture(SR.HttpHandlerBase_InvalidHttpMethodFormat, context.Request.HttpMethod));
        }
    }
}
