// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
    using System.Configuration;

    public class NameValueConfigurationElement
        : ConfigurationElement, IKeyedConfigurationElement<string>
    {
        const string NameName_ = "name";
        const string ValueName_ = "value";

        static readonly ConfigurationProperty NameProperty_
            = new ConfigurationProperty(
                NameName_,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
        static readonly ConfigurationProperty ValueProperty_
            = new ConfigurationProperty(
                ValueName_,
                typeof(String),
                default(String),
                ConfigurationPropertyOptions.IsRequired);

        string _name;
        bool _nameSet = false;
        string _value;
        bool _valueSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public NameValueConfigurationElement()
            : base()
        {
            _properties.Add(NameProperty_);
            _properties.Add(ValueProperty_);
        }

        public string Key { get { return Name; } }

        public string Name
        {
            get { return _nameSet ? _name : (string)base[NameProperty_]; }
            set
            {
                _name = value;
                _nameSet = true;
            }
        }

        public string Value
        {
            get { return _valueSet ? _value : (string)base[ValueProperty_]; }
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
