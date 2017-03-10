// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Diagnostics;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Narvalo.Web.Properties;

    /// <summary>
    /// Represents a custom HTTP handler that synchronously processes HTTP Web requests.
    /// </summary>
    public abstract partial class HttpHandler : IHttpHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class.
        /// </summary>
        protected HttpHandler() { }

        /// <summary>
        /// Gets or sets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// The default is false.
        /// </summary>
        /// <value>true if the <see cref="IHttpHandler"/> instance is reusable; otherwise, false.</value>
        public bool IsReusable { get; protected set; }

        public abstract HttpVerbs AcceptedVerbs { get; }

        /// <summary>
        /// Gets or sets a value indicating whether IIS 7.0 custom errors are disabled.
        /// The default is true.
        /// </summary>
        /// <value>true to disable IIS custom errors; otherwise, false.</value>
        public bool TrySkipIisCustomErrors { get; protected set; }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements
        /// the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> that provides references
        /// to the intrinsic server objects (for example, Request, Response, Session, and Server)
        /// used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            Require.NotNull(context, nameof(context));

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

        protected virtual void OnInvalidHttpMethod(HttpContext context)
        {
            Require.NotNull(context, nameof(context));

            var response = context.Response;

            // TODO: Check the error message.
            response.SetStatusCode(HttpStatusCode.MethodNotAllowed);
            response.Write(
                Format.Current(
                    Strings.HttpHandlerBase_InvalidHttpMethod_Format,
                    context.Request.HttpMethod,
                    AcceptedVerbs.ToString()));
        }

        private bool ValidateHttpMethod(HttpRequest request)
        {
            Debug.Assert(request != null);

            var verbs = ParseTo.Enum<HttpVerbs>(request.HttpMethod);

            return verbs.HasValue ? AcceptedVerbs.Contains(verbs.Value) : false;
        }
    }
}
