// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    public sealed class AssetSection : ConfigurationSection
    {
        public const string DefaultName = "assets";

        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        private static ConfigurationProperty s_DefaultProvider
            = new ConfigurationProperty(
                "defaultProvider",
                typeof(String),
                "DefaultAssetProvider",
               null,
               new StringValidator(1),
               ConfigurationPropertyOptions.None);

        private static ConfigurationProperty s_Providers
            = new ConfigurationProperty(
                "providers",
                typeof(ProviderSettingsCollection));

        private readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        private string _defaultProvider;
        private bool _defaultProviderSet = false;
        private ProviderSettingsCollection _providers;
        private bool _providersSet = false;

        public AssetSection()
        {
            _properties.Add(s_DefaultProvider);
            _properties.Add(s_Providers);
        }

        public string DefaultProvider
        {
            get
            {
                return _defaultProviderSet ? _defaultProvider : (string)base[s_DefaultProvider];
            }

            set
            {
                _defaultProvider = value;
                _defaultProviderSet = true;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "[Intentionally] This property must be writable to be initialized by the framework.")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return _providersSet ? _providers : (ProviderSettingsCollection)base[s_Providers];
            }

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
