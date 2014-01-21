﻿namespace Narvalo.Web
{
    using System.Net;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;

    public abstract class HttpHandlerBase<TQuery> : HttpHandlerBase
    {
        protected abstract Outcome<TQuery> Bind(HttpRequest request);

        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpResponse response, Outcome<TQuery> outcome)
        {
            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(outcome.ErrorMessage);
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            DebugCheck.NotNull(context);

            // Liaison du modèle.
            var outcome = Bind(context.Request);
            if (outcome.Unsuccessful) {
                HandleBindingFailure(context.Response, outcome);
                return;
            }

            ProcessRequestCore(context, outcome.Value);
        }
    }
}
