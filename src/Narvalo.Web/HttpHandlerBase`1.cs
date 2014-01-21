namespace Narvalo.Web
{
    using System;
    using System.Net;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;

    public abstract class HttpHandlerBase<TQuery> : HttpHandlerBase
    {
        protected abstract Outcome<TQuery> Bind(HttpRequest request);

        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpResponse response, Exception exception)
        {
            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(exception.Message);
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            var outcome = Bind(context.Request);
            if (!outcome.Successful) {
                HandleBindingFailure(context.Response, outcome.Exception);
                return;
            }

            ProcessRequestCore(context, outcome.Value);
        }
    }
}
