namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class FacebookSection : ConfigurationSection
    {
        #region Fields

        public const string DefaultName = "facebook";
        
        // Noms des propriétés.
        private const string AppIdPropertyName = "appId";
        private const string AppSecretPropertyName = "appSecret";
        private const string TrustHttpXForwardedHeadersPropertyName = "trustHttpXForwardedHeaders";

        // Valeurs par défaut.
        private const bool TrustHttpXForwardedHeadersDefaultValue = false;

        // Configuration des propriétés.
        private static ConfigurationProperty AppIdProperty
			= new ConfigurationProperty(
                AppIdPropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);
        private static ConfigurationProperty AppSecretProperty
			= new ConfigurationProperty(
                AppSecretPropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);
        private static ConfigurationProperty TrustHttpXForwardedHeadersProperty
			= new ConfigurationProperty(
                TrustHttpXForwardedHeadersPropertyName, 
                typeof(Boolean),
                TrustHttpXForwardedHeadersDefaultValue);

        // Champs pour utiliser manuellement les accesseurs.
        private string _appId;
        private bool _appIdSet = false;
        private string _appSecret;
        private bool _appSecretSet = false;
        private bool _trustHttpXForwardedHeaders;
        private bool _trustHttpXForwardedHeadersSet = false;

        // Stockage des propriétés.
        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public FacebookSection()
            : base()
        {
            _properties.Add(AppIdProperty);
            _properties.Add(AppSecretProperty);
            _properties.Add(TrustHttpXForwardedHeadersProperty);
        }

        public string AppId
        {
            get { return _appIdSet ? _appId : (string)base[AppIdProperty]; }
            set
            {
                _appId = value;
                _appIdSet = true;
            }
        }

        public string AppSecret
        {
            get { return _appSecretSet ? _appSecret : (string)base[AppSecretProperty]; }
            set
            {
                _appSecret = value;
                _appSecretSet = true;
            }
        }

        public bool TrustHttpXForwardedHeaders
        {
            get
            {
                return _trustHttpXForwardedHeadersSet
                    ? _trustHttpXForwardedHeaders
                    : (bool)base[TrustHttpXForwardedHeadersProperty];
            }
            set
            {
                _trustHttpXForwardedHeaders = value;
                _trustHttpXForwardedHeadersSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
