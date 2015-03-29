// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Narvalo.Fx.Extensions;
    using Narvalo.Web.Properties;

    public abstract class HttpHandlerBase : IHttpHandler
    {
        protected HttpHandlerBase() { }

        public virtual bool IsReusable
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() == false);

                return false;
            }
        }

        protected abstract HttpVerbs AcceptedVerbs { get; }

        protected virtual bool TrySkipIisCustomErrors
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() == true);

                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            Require.NotNull(context, "context");

            context.Response.TrySkipIisCustomErrors = TrySkipIisCustomErrors;

            if (ValidateHttpMethod(context.Request))
            {
                ProcessRequestCore(context);
            }
            else
            {
                OnInvalidHttpMethod(context);
            }
        }

        protected abstract void ProcessRequestCore(HttpContext context);

        protected virtual bool ValidateHttpMethod(HttpRequest request)
        {
            Require.NotNull(request, "request");

            return (from _ in ParseTo.Enum<HttpVerbs>(request.HttpMethod)
                    select AcceptedVerbs.Contains(_)) ?? false;
        }

        protected virtual void OnInvalidHttpMethod(HttpContext context)
        {
            Require.NotNull(context, "context");

            var response = context.Response;

            // TODO: Check the error message.
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write(
                Format.Resource(
                    Strings_Web.HttpHandlerBase_InvalidHttpMethod_Format,
                    context.Request.HttpMethod,
                    AcceptedVerbs.ToString()));
        }
    }
}
