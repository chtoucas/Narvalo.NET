namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class ParserFiltersSection : ConfigurationSection
    {
        #region Fields

        public const string DefaultName = "parserFilters";

        // Nom des propriétés.
        const string ParserFiltersName = "parserFilters";

        // Configuration des propriétés.
        static ConfigurationProperty ParserFiltersProperty
           = new ConfigurationProperty(
               ParserFiltersName,
               typeof(ParserFilterElementCollection),
               null,
               ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsDefaultCollection);

        // Champs pour utiliser manuellement les accesseurs.
        ParserFilterElementCollection _parserFilters;
        bool _parserFiltersSet = false;

        // Stockage des propriétés.
        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public ParserFiltersSection()
            : base()
        {
            _properties.Add(ParserFiltersProperty);
        }

        public ParserFilterElementCollection ParserFilters
        {
            get
            {
                return _parserFiltersSet
                    ? _parserFilters
                    : (ParserFilterElementCollection)base[ParserFiltersName];
            }
            set
            {
                _parserFilters = value;
                _parserFiltersSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
