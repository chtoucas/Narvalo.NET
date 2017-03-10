// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Web;

    using Narvalo.Web.Properties;

    public abstract partial class HttpHandler<TQuery, TBinder> : HttpHandler
        where TBinder : IHttpQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected sealed override void ProcessRequestCore(HttpContext context)
        {
            // REVIEW: Parameter validation.
            var binder = new TBinder();

            binder.Bind(context.Request)
                .Do(
                    onSome: q => ProcessRequestCore(context, q),
                    onNone: () => ProcessBindingFailure(context, binder));
        }

        protected virtual void OnBindingFailure(HttpContext context, HttpQueryBinderException exception)
        {
            Require.NotNull(context, nameof(context));
            Require.NotNull(exception, nameof(exception));

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }

        private void ProcessBindingFailure(HttpContext context, TBinder binder)
        {
            Debug.Assert(context != null);

            var errors = binder.BindingErrors;
            HttpQueryBinderException exception;

            var errorsCount = errors.Count();

            if (errorsCount > 1)
            {
                exception = new HttpQueryBinderException(
                    Strings.HttpHandlerBase_BindingFailure,
                    new AggregateException(errors));
            }
            else if (errorsCount == 1)
            {
                // FIXME: Handle null.
                exception = errors.First();
            }
            else
            {
                exception = new HttpQueryBinderException(Strings.HttpHandlerBase_UnknownBindingFailure);
            }

            OnBindingFailure(context, exception);
        }
    }
}
