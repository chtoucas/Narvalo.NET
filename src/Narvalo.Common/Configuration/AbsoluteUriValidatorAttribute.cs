namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AbsoluteUriValidatorAttribute : ConfigurationValidatorAttribute
    {
        public override ConfigurationValidatorBase ValidatorInstance
        {
            get { return new AbsoluteUriValidator(); }
        }
    }
}
