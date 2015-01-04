// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System.Configuration;

    public class ObjectConfigurationElement<TValue>
        : ConfigurationElement, IKeyedConfigurationElement<TValue>
    {
        private const string ValueName = "value";

        private static readonly ConfigurationProperty ValueProperty
			= new ConfigurationProperty(
                ValueName,
                typeof(TValue),
                default(TValue),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

        private TValue _value;
        private bool _valueSet = false;

        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public ObjectConfigurationElement()
            : base()
        {
            _properties.Add(ValueProperty);
        }

        public TValue Key { get { return Value; } }

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

