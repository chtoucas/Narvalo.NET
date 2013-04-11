namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using Narvalo.Resources;

    public class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        public override void Validate(object value)
        {
            string uriString = (string)value;
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute)) {
                throw ExceptionFactory.ConfigurationErrors(
                    SR.Configuration_UriIsNotAbsolute,
                    uriString);
            }
        }
    }
}
