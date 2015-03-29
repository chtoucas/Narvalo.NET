// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Properties;

    /// <summary>
    /// Represents a class that validates an absolute URI value.
    /// </summary>
    public sealed class AbsoluteUriValidator : ConfigurationValidatorBase
    {
        /// <copydoc cref="System.Configuration.ConfigurationValidatorBase.CanValidate"/>
        [SuppressMessage("Gendarme.Rules.Portability", "MonoCompatibilityReviewRule",
            Justification = "[Intentionally] Method marked as MonoTODO.")]
        public override bool CanValidate(Type type)
        {
            return type == typeof(Uri);
        }

        /// <copydoc cref="System.Configuration.ConfigurationValidatorBase.Validate"/>
        public override void Validate(object value)
        {
            string uriString = (string)value;

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute)) {
                throw new ConfigurationErrorsException(
                    Format.Resource(Strings_Common.AbsoluteUriValidator_UriIsNotAbsolute_Format, uriString));
            }
        }
    }
}
