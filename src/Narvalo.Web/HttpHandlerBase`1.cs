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

        protected virtual void HandleBindingFailure(HttpResponse response, Error error)
        {
            response.SetStatusCode(HttpStatusCode.BadRequest);
            response.Write(error.Message);
        }

        protected Outcome<TQuery> BindingFailure(string paramName)
        {
            return Outcome<TQuery>.Failure(
                String.Format(
                    CultureInfo.CurrentCulture, 
                    SR.HttpHandlerBase_MissingOrInvalidParameterFormat,
                    paramName));
        }

        protected override void ProcessRequestCore(HttpContext context)
        {
            Requires.NotNull(context, "context");

            // Liaison du modèle.
            var outcome = Bind(context.Request);
            if (outcome.Unsuccessful) {
                HandleBindingFailure(context.Response, outcome.Error);
                return;
            }
            var query = outcome.Value;

            ProcessRequestCore(context, query);
        }
    }
}
