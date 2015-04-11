// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Configuration
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public sealed partial class AssetSection : ConfigurationSection
    {
        public const string DefaultName = "assets";

        public static readonly string SectionName = NarvaloWebSectionGroup.GroupName + "/" + DefaultName;

        internal const string DefaultProviderPropertyName = "defaultProvider";
        internal const string ProvidersPropertyName = "providers";

        private static readonly ConfigurationPropertyCollection s_Properties;

        private static readonly ConfigurationProperty s_DefaultProvider
            = new ConfigurationProperty(
                DefaultProviderPropertyName,
                typeof(String),
                "DefaultAssetProvider" /* defaultValue */,
               null /* typeConverter */,
               new StringValidator(1),
               ConfigurationPropertyOptions.None);

        private static readonly ConfigurationProperty s_Providers
            = new ConfigurationProperty(
                ProvidersPropertyName,
                typeof(ProviderSettingsCollection));

        private string _defaultProvider;
        private ProviderSettingsCollection _providers;

        static AssetSection()
        {
            s_Properties = new ConfigurationPropertyCollection();
            s_Properties.Add(s_DefaultProvider);
            s_Properties.Add(s_Providers);
        }

        public string DefaultProvider
        {
            get
            {
                return _defaultProvider ?? (string)base[s_DefaultProvider];
            }

            set
            {
                Require.PropertyNotWhiteSpace(value);

                _defaultProvider = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
            Justification = "[Intentionally] This property must be writable in order to be initialized by the framework.")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return _providers ?? (ProviderSettingsCollection)base[s_Providers];
            }

            set
            {
                Require.Property(value);

                _providers = value;
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                Contract.Ensures(Contract.Result<ConfigurationPropertyCollection>() != null);

                return s_Properties;
            }
        }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.
    
namespace Narvalo.Web.Configuration
{
    using System.Configuration;
    using System.Diagnostics.Contracts;

    public sealed partial class AssetSection : ConfigurationSection
    {
        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_properties != null);
        }
    }
}

#endif
