namespace Narvalo.Web
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpContext context, QueryBinderException exception)
        {
            DebugCheck.NotNull(context);

            var response = context.Response;

            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var binder = new TBinder();

            TQuery query = binder.Bind(context.Request);

            if (!binder.Successful) {
                var errors = binder.BindingErrors;
                QueryBinderException exception;

                if (errors.Count > 1) {
                   exception = new QueryBinderException(SR.HttpHandlerBase_InvalidRequest, new AggregateException(errors));
                }
                else if (errors.Count == 1) {
                    exception = errors.First();
                }
                else {
                    exception = new QueryBinderException(SR.HttpHandlerBase_InvalidRequest);
                }

                HandleBindingFailure(context, exception);
                return;
            }

            ProcessRequestCore(context, query);
        }
    }
}
