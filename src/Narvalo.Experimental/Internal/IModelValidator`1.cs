namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IModelValidator<TModel>
    {
        IEnumerable<ValidationResult> Validate(TModel model);
    }
}
