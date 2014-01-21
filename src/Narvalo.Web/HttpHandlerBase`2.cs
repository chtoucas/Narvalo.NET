namespace Narvalo.Web
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IHttpQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpContext context, HttpQueryBinderException exception)
        {
            DebugCheck.NotNull(context);

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }

        protected sealed override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var binder = new TBinder();

            TQuery query = binder.Bind(context.Request);

            if (binder.Successful) {
                ProcessRequestCore(context, query);
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
    }
}
