namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class XmlValidationSection : ConfigurationSection
    {
        #region Fields

        public const string DefaultName = "xmlValidation";

        // Noms des attributs.
        private const string RendererTypePropertyName = "rendererType";
        private const string ValidationHeaderPropertyName = "validationHeader";

        // Configuration des attributs.
        private static ConfigurationProperty RendererTypeProperty
			= new ConfigurationProperty(
                RendererTypePropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        private static ConfigurationProperty ValidationHeaderProperty
			= new ConfigurationProperty(
                ValidationHeaderPropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        // Champs pour utiliser manuellement les accesseurs.
        private string _renderType;
        private bool _renderTypeSet = false;
        private string _validationHeader;
        private bool _validationHeaderSet = false;

        // Stockage des propriétés.
        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public XmlValidationSection()
            : base()
        {
            _properties.Add(RendererTypeProperty);
            _properties.Add(ValidationHeaderProperty);
        }

        public string RendererType
        {
            get { return _renderTypeSet ? _renderType : (string)base[RendererTypeProperty]; }
            set
            {
                _renderType = value;
                _renderTypeSet = true;
            }
        }

        public string ValidationHeader
        {
            get { return _validationHeaderSet ? _validationHeader : (string)base[ValidationHeaderProperty]; }
            set
            {
                _validationHeader = value;
                _validationHeaderSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
