namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web;

    public abstract class HttpHandlerBase<TQuery, TBinder> : HttpHandlerBase
        where TBinder : IQueryBinder<TQuery>, new()
    {
        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpResponse response, QueryBinderException exception)
        {
            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var binder = new TBinder();

            TQuery query = binder.Bind(context.Request);
            
            if (!binder.Successful) {
                var exception = new QueryBinderException(SR.HttpHandlerBase_InvalidRequest, new AggregateException(binder.BindingErrors));
                HandleBindingFailure(context.Response, exception);
                return;
            }

            ProcessRequestCore(context, query);
        }
    }
}
