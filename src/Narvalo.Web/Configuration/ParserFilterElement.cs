namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Web.UI;
    using Narvalo.Configuration;

    public class ParserFilterElement : ConfigurationElement, IKeyedConfigurationElement<Type>
    {
        #region Fields

        // Nom des propriétés.
        private const string ElementTypeName = "type";

        // Configuration des propriétés.
        private static readonly ConfigurationProperty ElementTypeProperty
			= new ConfigurationProperty(
                ElementTypeName,
                typeof(Type),
                null,
                new TypeNameConverter(),
                new SubclassTypeValidator(typeof(PageParserFilter)),
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

        // Champs pour utiliser manuellement les accesseurs.
        private Type _elementType;
        private bool _elementTypeSet = false;

        // Stockage des propriétés.
        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public ParserFilterElement()
            : base()
        {
            _properties.Add(ElementTypeProperty);
        }

        public Type Key { get { return ElementType; } }

        public Type ElementType
        {
            get { return _elementTypeSet ? _elementType : (Type)base[ElementTypeName]; }
            set
            {
                Requires.NotNull(value, "value");

                _elementType = value;
                _elementTypeSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
