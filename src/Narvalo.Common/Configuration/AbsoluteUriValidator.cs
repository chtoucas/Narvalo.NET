// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    using Narvalo.Properties;

    /// <summary>
    /// Represents a class that validates an absolute URI value.
    /// </summary>
    public sealed class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        /// <inheritdoc cref="System.Configuration.ConfigurationValidatorBase.CanValidate"/>
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        /// <inheritdoc cref="System.Configuration.ConfigurationValidatorBase.Validate"/>
        public override void Validate(object value)
        {
            string uriString = (string)value;

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute)) {
                throw new ConfigurationErrorsException(
                    Format.Current(Strings_Common.AbsoluteUriValidator_UriIsNotAbsolute_Format, uriString));
            }
        }
    }
}
