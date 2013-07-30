namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;

    public class AssetSection : ConfigurationSection
    {
        #region > Champs <

        public const string DefaultName = "assets";

        // Noms des propriétés.
        const string DefaultProviderPropertyName = "defaultProvider";
        const string ProvidersPropertyName = "providers";

        // Valeurs par défaut.
        const string DefaultProviderDefaultValue = "DefaultAssetProvider";

        // Configuration des propriétés.
        static ConfigurationProperty DefaultProviderProperty
            = new ConfigurationProperty(
               DefaultProviderPropertyName,
               typeof(String),
               DefaultProviderDefaultValue,
               null,
               new StringValidator(1),
               ConfigurationPropertyOptions.None);
        static ConfigurationProperty ProvidersProperty
            = new ConfigurationProperty(ProvidersPropertyName, typeof(ProviderSettingsCollection));

        // Champs pour utiliser manuellement les accesseurs.
        string _defaultProvider;
        bool _defaultProviderSet = false;
        ProviderSettingsCollection _providers;
        bool _providersSet = false;

        // Stockage des propriétés.
        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        #endregion

        public AssetSection()
            : base()
        {
            _properties.Add(DefaultProviderProperty);
            _properties.Add(ProvidersProperty);
        }

        public string DefaultProvider
        {
            get { return _defaultProviderSet ? _defaultProvider : (string)base[DefaultProviderProperty]; }
            set
            {
                _defaultProvider = value;
                _defaultProviderSet = true;
            }
        }

        public ProviderSettingsCollection Providers
        {
            get { return _providersSet ? _providers : (ProviderSettingsCollection)base[ProvidersProperty]; }
            set
            {
                _providers = value;
                _providersSet = true;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
