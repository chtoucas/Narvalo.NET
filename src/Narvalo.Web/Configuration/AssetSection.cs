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

        static ConfigurationProperty DefaultProvider_
            = new ConfigurationProperty(
                "defaultProvider", 
                typeof(String), 
                "DefaultAssetProvider",
               null, 
               new StringValidator(1), 
               ConfigurationPropertyOptions.None);
        
        static ConfigurationProperty Providers_
            = new ConfigurationProperty(
                "providers", 
                typeof(ProviderSettingsCollection));

        string _defaultProvider;
        ProviderSettingsCollection _providers;

        bool _defaultProviderSet = false;
        bool _providersSet = false;

        ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public AssetSection()
        {
            _properties.Add(DefaultProvider_);
            _properties.Add(Providers_);
        }

        public string DefaultProvider
        {
            get { 
                return _defaultProviderSet ? _defaultProvider : (string)base[DefaultProvider_]; 
            }
            
            set { 
                _defaultProvider = value; 
                _defaultProviderSet = true; 
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "ProviderSettingsCollection précède les classes génériques.")]
        public ProviderSettingsCollection Providers
        {
            get { 
                return _providersSet ? _providers : (ProviderSettingsCollection)base[Providers_];
            }
            
            set { 
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
