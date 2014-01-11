namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class GoogleApiElement : ConfigurationElement
    {
        #region > Champs <

        // Nom des attributs.
        private const string ApiKeyPropertyName = "apiKey";

        // Configuration des attributs.
        private static ConfigurationProperty ApiKeyProperty
			= new ConfigurationProperty(
                ApiKeyPropertyName, typeof(String), null, ConfigurationPropertyOptions.IsRequired);

        // Champs pour utiliser manuellement les accesseurs.
        private string _apiKey;
        private bool _apiKeySet = false;

        // Stockage des propriétés.
        private ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public GoogleApiElement()
            : base()
        {
            _properties.Add(ApiKeyProperty);
        }

        public string ApiKey
        {
            get { return _apiKeySet ? _apiKey : (string)base[ApiKeyProperty]; }
            set
            {
                _apiKey = value;
                _apiKeySet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }

}
