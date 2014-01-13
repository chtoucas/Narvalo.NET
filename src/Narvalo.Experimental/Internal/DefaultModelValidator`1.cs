namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DefaultModelValidator<TModel> : IModelValidator<TModel>
    {
        #region IModelValidator<TModel>

        public IEnumerable<ValidationResult> Validate(TModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null /* serviceProvider */, null /* items */);
            var isValid = Validator.TryValidateObject(
                model, validationContext, validationResults, true /* validateAllProperties */);

            return validationResults;
        }

        #endregion
    }
}
