namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public sealed class OptimizationSection : ConfigurationSection
    {
        public const string DefaultName = "optimization";
        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        static ConfigurationProperty EnableWhiteSpaceBusting_
            = new ConfigurationProperty("enableWhiteSpaceBusting", typeof(Boolean), true, ConfigurationPropertyOptions.IsRequired);

        bool _enableWhiteSpaceBusting;

        bool _enableWhiteSpaceBustingSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public OptimizationSection()
        {
            _properties.Add(EnableWhiteSpaceBusting_);
        }

        public bool EnableWhiteSpaceBusting
        {
            get { 
                return _enableWhiteSpaceBustingSet ? _enableWhiteSpaceBusting : (bool)base[EnableWhiteSpaceBusting_]; 
            }
            
            set { 
                _enableWhiteSpaceBusting = value; 
                _enableWhiteSpaceBustingSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
