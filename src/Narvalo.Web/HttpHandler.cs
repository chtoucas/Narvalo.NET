// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using Narvalo.Fx;
    using Narvalo.Web.Properties;

    /// <summary>
    /// Represents a custom HTTP handler that synchronously processes HTTP Web requests.
    /// </summary>
    public abstract partial class HttpHandler : IHttpHandler
    {
        private bool _isReusable;
        private bool _trySkipIIsCustomErrors = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class.
        /// </summary>
        protected HttpHandler() { }

        /// <summary>
        /// Gets or sets a value indicating whether another request can use the <see cref="IHttpHandler"/> instance.
        /// The default is <see langword="false"/>.
        /// </summary>
        /// <value><see langword="true"/> if the <see cref="IHttpHandler"/> instance is reusable; otherwise, <see langword="false"/>.</value>
        public bool IsReusable
        {
            get { return _isReusable; }
            protected set { _isReusable = value; }
        }

        public abstract HttpVerbs AcceptedVerbs { get; }

        /// <summary>
        /// Gets or sets a value indicating whether IIS 7.0 custom errors are disabled.
        /// The default is <see langword="true"/>.
        /// </summary>
        /// <value><see langword="true"/> to disable IIS custom errors; otherwise, <see langword="false"/>.</value>
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
            Demand.NotNull(request);

            return (from _ in ParseTo.Enum<HttpVerbs>(request.HttpMethod)
                    select AcceptedVerbs.Contains(_)) ?? false;
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Web
{
    using System.Diagnostics.Contracts;
    using System.Web;

    [ContractClass(typeof(HttpHandlerContract))]
    public abstract partial class HttpHandler { }

    [ContractClassFor(typeof(HttpHandler))]
    internal abstract class HttpHandlerContract : HttpHandler
    {
        protected override void ProcessRequestCore(HttpContext context)
        {
            Contract.Requires(context != null);
        }
    }
}

#endif
