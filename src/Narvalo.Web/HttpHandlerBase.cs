// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Diagnostics.Contracts;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Narvalo.Fx.Extensions;
    using Narvalo.Web.Properties;

    /// <summary>
    /// Represents a custom HTTP handler that synchronously processes HTTP Web requests.
    /// </summary>
    public abstract partial class HttpHandlerBase : IHttpHandler
    {
        private bool _isReusable = false;
        private bool _trySkipIIsCustomErrors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandlerBase"/> class.
        /// </summary>
        protected HttpHandlerBase() { }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// The default is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable
        {
            get { return _isReusable; }
            protected set { _isReusable = value; }
        }

        public abstract HttpVerbs AcceptedVerbs { get; }

        /// <summary>
        /// Gets or sets a value that specifies whether IIS 7.0 custom errors are disabled.
        /// The default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> to disable IIS custom errors; otherwise, <c>false</c>.</value>
        public bool TrySkipIisCustomErrors
        {
            get { return _trySkipIIsCustomErrors; }
            protected set { _trySkipIIsCustomErrors = value; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements
        /// the <see cref="IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="HttpContext"/> that provides references
        /// to the intrinsic server objects (for example, Request, Response, Session, and Server)
        /// used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            Require.NotNull(context, "context");

            context.Response.TrySkipIisCustomErrors = TrySkipIisCustomErrors;

            if (ValidateHttpMethod_(context.Request))
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

        private bool ValidateHttpMethod_(HttpRequest request)
        {
            Contract.Requires(request != null);

            return (from _ in ParseTo.Enum<HttpVerbs>(request.HttpMethod)
                    select AcceptedVerbs.Contains(_)) ?? false;
        }
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(HttpHandlerBaseContract))]
    public abstract partial class HttpHandlerBase { }

    [ContractClassFor(typeof(HttpHandlerBase))]
    internal abstract class HttpHandlerBaseContract : HttpHandlerBase
    {
        protected override void ProcessRequestCore(HttpContext context)
        {
            Contract.Requires(context != null);
        }
    }

#endif
}
