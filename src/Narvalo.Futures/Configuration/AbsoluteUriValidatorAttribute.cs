// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Instruit le framework .NET de vérifier qu'une propriété de configuration représente bien une URI absolue.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AbsoluteUriValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance => new AbsoluteUriValidator();
    }
}
