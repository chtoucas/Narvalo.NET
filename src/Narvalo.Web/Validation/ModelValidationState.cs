namespace Narvalo.Web.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ModelValidationState
    {
        readonly bool _isValid;
        readonly IEnumerable<ValidationResult> _validationResults;

        public ModelValidationState(bool isValid, IEnumerable<ValidationResult> validationResults)
        {
            _isValid = isValid;
            _validationResults = validationResults;
        }

        public bool IsValid { get { return _isValid; } }

        public IEnumerable<ValidationResult> ValidationResults { get { return _validationResults; } }
    }
}
