namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class ParserFiltersSection : ConfigurationSection
    {
        public const string DefaultName = "parserFilters";
        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        static ConfigurationProperty ParserFilters_
           = new ConfigurationProperty(
               "parserFilters",
               typeof(ParserFilterElementCollection),
               null,
               ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsDefaultCollection);

        ParserFilterElementCollection _parserFilters;

        bool _parserFiltersSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public ParserFiltersSection()
            : base()
        {
            _properties.Add(ParserFilters_);
        }

        public ParserFilterElementCollection ParserFilters
        {
            get { return _parserFiltersSet ? _parserFilters : (ParserFilterElementCollection)base[ParserFilters_]; }
            set { _parserFilters = value; _parserFiltersSet = true; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
