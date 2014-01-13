namespace Narvalo.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

    public class NameObjectConfigurationElement<TValue>
        : ConfigurationElement, IKeyedConfigurationElement<String>
    {
        private const string NameName = "name";
        private const string ValueName = "value";

        private static readonly ConfigurationProperty NameProperty
			= new ConfigurationProperty(
                NameName,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
        private static readonly ConfigurationProperty ValueProperty
			= new ConfigurationProperty(
                ValueName,
                typeof(TValue),
                default(TValue),
                ConfigurationPropertyOptions.IsRequired);

        private string _name;
        private bool _nameSet = false;
        private TValue _value;
        private bool _valueSet = false;

        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public NameObjectConfigurationElement()
            : base()
        {
            _properties.Add(NameProperty);
            _properties.Add(ValueProperty);
        }

        public string Key { get { return Name; } }

        public string Name
        {
            get { return _nameSet ? _name : (string)base[NameProperty]; }
            set
            {
                _name = value;
                _nameSet = true;
            }
        }

        public TValue Value
        {
            get { return _valueSet ? _value : (TValue)base[ValueProperty]; }
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
