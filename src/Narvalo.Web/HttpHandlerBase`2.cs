// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Web;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IHttpQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected sealed override void ProcessRequestCore(HttpContext context)
        {
            // REVIEW: Parameter validation.
            var binder = new TBinder();

            binder.Bind(context.Request)
                .Invoke(
                    action: _ => ProcessRequestCore(context, _),
                    caseNone: () => OnBindingFailure(context, binder));
        }

        protected void OnBindingFailure(HttpContext context, TBinder binder)
        {
            Contract.Requires(context != null);

            var errors = binder.BindingErrors;
            HttpQueryBinderException exception;

            var errorsCount = errors.Count();

            if (errorsCount > 1)
            {
                exception = new HttpQueryBinderException(Strings_Web.HttpHandlerBase_InvalidRequest, new AggregateException(errors));
            }
            else if (errorsCount == 1)
            {
                exception = errors.First();
            }
            else
            {
                exception = new HttpQueryBinderException(Strings_Web.HttpHandlerBase_InvalidRequest);
            }

            OnBindingFailureCore(context, exception);
        }

        protected virtual void OnBindingFailureCore(HttpContext context, HttpQueryBinderException exception)
        {
            Require.NotNull(context, "context");
            Require.NotNull(exception, "exception");

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }
    }
}
