// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Narvalo.Fx;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IHttpQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected sealed override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var binder = new TBinder();

            binder.Bind(context.Request)
                .OnSome(_ => ProcessRequestCore(context, _))
                .OnNone(() => OnBindingFailure(context, binder));
        }

        protected void OnBindingFailure(HttpContext context, TBinder binder)
        {
            var errors = binder.BindingErrors;
            HttpQueryBinderException exception;

            var errorsCount = errors.Count();

            if (errorsCount > 1) {
                exception = new HttpQueryBinderException(SR.HttpHandlerBase_InvalidRequest, new AggregateException(errors));
            }
            else if (errorsCount == 1) {
                exception = errors.First();
            }
            else {
                exception = new HttpQueryBinderException(SR.HttpHandlerBase_InvalidRequest);
            }

            OnBindingFailureCore(context, exception);
        }

        protected virtual void OnBindingFailureCore(HttpContext context, HttpQueryBinderException exception)
        {
            DebugCheck.NotNull(context);

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }
    }
}
