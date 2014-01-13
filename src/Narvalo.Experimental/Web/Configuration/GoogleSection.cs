namespace Narvalo.Web.Configuration
{
    using System.Configuration;

    public class GoogleSection : ConfigurationSection
    {
        #region Fields

        public const string DefaultName = "google";
        
        // Nom des propriétés.
        private const string AdSensePropertyName = "adSense";
        private const string AnalyticsPropertyName = "analytics";
        private const string MapsPropertyName = "maps";

        // Configuration des propriétés.
        private static ConfigurationProperty AdSenseProperty
			= new ConfigurationProperty(AdSensePropertyName, typeof(GoogleApiElement), null);
        private static ConfigurationProperty AnalyticsProperty
			= new ConfigurationProperty(AnalyticsPropertyName, typeof(GoogleApiElement), null);
        private static ConfigurationProperty MapsProperty
			= new ConfigurationProperty(MapsPropertyName, typeof(GoogleApiElement), null);

        // Champs pour utiliser manuellement les accesseurs.
        private GoogleApiElement _adSense;
        private bool _adSenseSet = false;
        private GoogleApiElement _analytics;
        private bool _analyticsSet = false;
        private GoogleApiElement _maps;
        private bool _mapsSet = false;

        // Stockage des propriétés.
        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public GoogleSection()
            : base()
        {
            _properties.Add(AdSenseProperty);
            _properties.Add(AnalyticsProperty);
            _properties.Add(MapsProperty);
        }

        public GoogleApiElement AdSense
        {
            get { return _adSenseSet ? _adSense : (GoogleApiElement)base[AdSenseProperty]; }
            set
            {
                _adSense = value;
                _adSenseSet = true;
            }
        }

        public GoogleApiElement Analytics
        {
            get { return _analyticsSet ? _analytics : (GoogleApiElement)base[AnalyticsProperty]; }
            set
            {
                _analytics = value;
                _analyticsSet = true;
            }
        }

        public GoogleApiElement Maps
        {
            get { return _mapsSet ? _maps : (GoogleApiElement)base[MapsPropertyName]; }
            set
            {
                _maps = value;
                _mapsSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}

