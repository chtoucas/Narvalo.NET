namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Classe permettant de valider qu'une valeur représente bien une URI absolue.
    /// </summary>
    public class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        /// <summary />
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        /// <summary />
        public override void Validate(object value)
        {
            string uriString = (string)value;
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute)) {
                throw Failure.ConfigurationErrors(
                    SR.AbsoluteUriValidator_UriIsNotAbsoluteFormat,
                    uriString);
            }
        }
    }
}
