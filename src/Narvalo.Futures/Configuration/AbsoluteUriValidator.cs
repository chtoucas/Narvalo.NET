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
        public override bool CanValidate(Type type) => type == typeof(Uri);

        public override void Validate(object value)
        {
            string uriString = (string)value;

            if (!Uri.IsWellFormedUriString(uriString, UriKind.Absolute))
            {
                throw new ConfigurationErrorsException(
                    Format.Current(Strings_Futures.AbsoluteUriValidator_UriIsNotAbsolute_Format, uriString));
            }
        }
    }
}
