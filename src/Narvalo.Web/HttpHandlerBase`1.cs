namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;

    public abstract class HttpHandlerBase<TQuery> : HttpHandlerBase
    {
        protected abstract Outcome<TQuery> Bind(HttpRequest request);

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

            var outcome = Bind(context.Request);

            if (!outcome.Successful) {
                HandleBindingFailure(context, new QueryBinderException(outcome.Exception.Message, outcome.Exception));
                return;
            }

            ProcessRequestCore(context, outcome.Value);
        }
    }
}
