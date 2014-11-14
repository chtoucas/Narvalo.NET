namespace Narvalo.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;

    public class KeyValuePairConfigurationElement<TKey, TValue>
        : ConfigurationElement, IKeyedConfigurationElement<TKey>
    {
        private const string KeyName = "key";
        private const string ValueName = "value";

        private static readonly ConfigurationProperty KeyProperty
			= new ConfigurationProperty(
                KeyName,
                typeof(TKey),
                default(TKey),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
        private static readonly ConfigurationProperty ValueProperty
			= new ConfigurationProperty(
                ValueName,
                typeof(TValue),
                default(TValue),
                ConfigurationPropertyOptions.IsRequired);

        private TKey _key;
        private bool _keySet = false;
        private TValue _value;
        private bool _valueSet = false;

        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public KeyValuePairConfigurationElement()
            : base()
        {
            _properties.Add(KeyProperty);
            _properties.Add(ValueProperty);
        }

        public TKey Key
        {
            get { return _keySet ? _key : (TKey)base[KeyProperty]; }
            set
            {
                _key = value;
                _keySet = true;
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
