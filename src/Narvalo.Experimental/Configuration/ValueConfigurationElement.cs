namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    public class ValueConfigurationElement
        : ConfigurationElement, IKeyedConfigurationElement<String>
    {
        private const string ValueName = "value";

        private static readonly ConfigurationProperty ValueProperty
			= new ConfigurationProperty(
                ValueName,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

        private string _value;
        private bool _valueSet = false;

        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public ValueConfigurationElement()
            : base()
        {
            _properties.Add(ValueProperty);
        }

        public string Key { get { return Value; } }

        public string Value
        {
            get { return _valueSet ? _value : (string)base[ValueProperty]; }
            set
            {
                _value = value;
                _valueSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}

