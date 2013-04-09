namespace Narvalo.Web
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Narvalo;
    using Narvalo.Fx;
    using Narvalo.Web.Validation;

    public abstract class HttpHandlerBase<TQuery> : HttpHandlerBase
    {
        protected abstract Outcome<TQuery> Bind(HttpRequest request);

        protected abstract void ProcessRequestCore(HttpContext context, TQuery query);

        protected override void ProcessRequestCore(HttpContext context)
        {
            Requires.NotNull(context, "context");

            // Liaison du modèle.
            var outcome = Bind(context.Request);
            if (outcome.Failed) {
                //ProcessFailure(outcome.Error);
                return;
            }
            var query = outcome.Value;

            // Validation du modèle.
            //var succeed = model.Match(_ => ValidateModel(context, _), false /* defaultValue */);
            var validationState = ValidateQuery(query);
            if (!validationState.IsValid) {
                //ProcessFailure(validationState.ValidationResults);
                return;
            }

            ProcessRequestCore(context, query);
        }

        protected ModelValidationState ValidateQuery(TQuery query)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(query, null /* serviceProvider */, null /* items */);
            var succeed = Validator.TryValidateObject(
                query, validationContext, validationResults, true /* validateAllProperties */);

            return new ModelValidationState(succeed, validationResults);
        }
    }
}
