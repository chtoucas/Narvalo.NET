namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class XmlValidationSection : ConfigurationSection
    {
        public const string DefaultName = "xmlValidation";
        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        static ConfigurationProperty RendererType_
           = new ConfigurationProperty(
               "rendererType", typeof(String), null, ConfigurationPropertyOptions.IsRequired);
        static ConfigurationProperty ValidationHeader_
           = new ConfigurationProperty(
               "validationHeader", typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        string _renderType;
        string _validationHeader;

        bool _renderTypeSet = false;
        bool _validationHeaderSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public XmlValidationSection()
            : base()
        {
            _properties.Add(RendererType_);
            _properties.Add(ValidationHeader_);
        }

        public string RendererType
        {
            get { return _renderTypeSet ? _renderType : (string)base[RendererType_]; }
            set { _renderType = value; _renderTypeSet = true; }
        }

        public string ValidationHeader
        {
            get { return _validationHeaderSet ? _validationHeader : (string)base[ValidationHeader_]; }
            set { _validationHeader = value; _validationHeaderSet = true; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
