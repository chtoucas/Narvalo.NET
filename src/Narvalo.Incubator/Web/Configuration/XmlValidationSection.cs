namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class XmlValidationSection : ConfigurationSection
    {
        #region Fields

        public const string DefaultName = "xmlValidation";

        // Noms des attributs.
        const string RendererTypePropertyName = "rendererType";
        const string ValidationHeaderPropertyName = "validationHeader";

        // Configuration des attributs.
        static ConfigurationProperty RendererTypeProperty
           = new ConfigurationProperty(
               RendererTypePropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        static ConfigurationProperty ValidationHeaderProperty
           = new ConfigurationProperty(
               ValidationHeaderPropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        // Champs pour utiliser manuellement les accesseurs.
        string _renderType;
        bool _renderTypeSet = false;
        string _validationHeader;
        bool _validationHeaderSet = false;

        // Stockage des propriétés.
        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

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
