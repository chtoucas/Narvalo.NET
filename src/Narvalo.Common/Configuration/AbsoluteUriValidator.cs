namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Represents a class that provides validation of an absolute URI value.
    /// </summary>
    public class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        /// <summary />
        /// <see cref="System.Configuration.ConfigurationValidatorBase.CanValidate"/>
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        /// <summary />
        /// <see cref="System.Configuration.ConfigurationValidatorBase.Validate"/>
        public override void Validate(object value)
        {
            string uriString = (string)value;
            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute)) {
                throw new ConfigurationErrorsException(
                    Format.CurrentCulture(SR.AbsoluteUriValidator_UriIsNotAbsoluteFormat, uriString));
            }
        }
    }
}
