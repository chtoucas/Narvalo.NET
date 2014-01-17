namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Classe permettant de valider qu'une valeur représente bien une URI absolue.
    /// </summary>
    public class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        /// <summary>
        /// Initialise un nouvel objet de type <see cref="Narvalo.Configuration.AbsoluteUriValidator"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

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
