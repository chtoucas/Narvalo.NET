// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Narvalo.Fx;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        public virtual bool IsReusable { get { return false; } }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected virtual bool TrySkipIisCustomErrors { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            Require.NotNull(context, "context");

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
            Require.NotNull(request, "request");

            return (from _ in ParseTo.Enum<HttpVerbs>(request.HttpMethod) select AcceptedVerbs.HasFlag(_)) ?? false;
        }

        protected virtual void OnInvalidHttpMethod(HttpContext context)
        {
            Require.NotNull(context, "context");

            var response = context.Response;

            // TODO: Indiquer les méthodes autorisées dans la réponse.
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write(Format.CurrentCulture(Strings_Web.HttpHandlerBase_InvalidHttpMethodFormat, context.Request.HttpMethod));
        }
    }
}
