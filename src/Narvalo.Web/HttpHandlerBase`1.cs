namespace Narvalo.Web
{
    using System;
    using System.Globalization;
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

        protected Outcome<TQuery> BindingFailure(string parameterName)
        {
            return Outcome.Failure<TQuery>(
                String.Format(
                    CultureInfo.CurrentCulture, 
                    SR.HttpHandlerBase_MissingOrInvalidParameterFormat,
                    parameterName));
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            Require.NotNull(context, "context");

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
