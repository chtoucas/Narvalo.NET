namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Web.UI;
    using Narvalo.Configuration;

    public class ParserFilterElement : ConfigurationElement, IKeyedConfigurationElement<Type>
    {
        static readonly ConfigurationProperty ElementType_
           = new ConfigurationProperty(
               "type",
               typeof(Type),
               null,
               new TypeNameConverter(),
               new SubclassTypeValidator(typeof(PageParserFilter)),
               ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

        Type _elementType;

        bool _elementTypeSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public ParserFilterElement()
            : base()
        {
            _properties.Add(ElementType_);
        }

        public Type Key { get { return ElementType; } }

        public Type ElementType
        {
            get { return _elementTypeSet ? _elementType : (Type)base[ElementType_]; }
            set { _elementType = Require.Property(value); _elementTypeSet = true; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
