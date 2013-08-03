namespace Narvalo.Web
{
    using System;
    //using System.Collections.Generic;
    //using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web.Resources;

    public abstract class HttpHandlerBase<TQuery> : HttpHandlerBase
    {
        protected abstract Outcome<TQuery> Bind(HttpRequest request);

        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected virtual void HandleBindingFailure(HttpResponse response, Error error)
        {
            // XXX: Est-ce le bon code HTTP ?
            response.SetStatusCode(HttpStatusCode.NotFound);
            response.Write(error.Message);
        }

        protected Outcome<TQuery> MissingOrInvalidParameterOutcome(string paramName)
        {
            return Outcome<TQuery>.Failure(
                String.Format(
                    CultureInfo.CurrentCulture, 
                    SR.HttpHandlerBase_MissingOrInvalidParameter,
                    paramName));
        }

        //protected virtual void HandleValidationFailure(
        //    HttpResponse response,
        //    IEnumerable<ValidationResult> errors)
        //{
        //    // XXX: Est-ce le bon code HTTP ?
        //    response.SetStatusCode(HttpStatusCode.NotFound);
        //    response.Write("XXX Invalid request received.");
        //}

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

            //// Validation du modèle.
            //var validationErrors = ValidateQuery_(query);
            //if (validationErrors.IsSome) {
            //    HandleValidationFailure(context.Response, validationErrors.Value);
            //    return;
            //}

            ProcessRequestCore(context, query);
        }

        //Maybe<List<ValidationResult>> ValidateQuery_(TQuery query)
        //{
        //    var validationResults = new List<ValidationResult>();
        //    var validationContext = new ValidationContext(query, null /* serviceProvider */, null /* items */);
        //    var succeed = Validator.TryValidateObject(
        //        query, validationContext, validationResults, true /* validateAllProperties */);

        //    return succeed ? Maybe<List<ValidationResult>>.None : Maybe.Create(validationResults);
        //}
    }
}
