namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Instruit le framework .NET de valider qu'une propriété de configuration représente bien une URI absolue.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AbsoluteUriValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get { return new AbsoluteUriValidator(); }
        }
    }
}
