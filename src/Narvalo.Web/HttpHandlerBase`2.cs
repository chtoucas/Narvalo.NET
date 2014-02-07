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

            Maybe<TQuery> query = binder.Bind(context.Request);

            if (query.IsSome) {
                ProcessRequestCore(context, query.Value);
            }
            else {
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

                HandleBindingFailure(context, exception);
            }
        }

        protected virtual void HandleBindingFailure(HttpContext context, HttpQueryBinderException exception)
        {
            DebugCheck.NotNull(context);

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }
    }
}
