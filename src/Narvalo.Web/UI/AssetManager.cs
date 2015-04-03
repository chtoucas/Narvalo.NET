// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;
    using System.Web.Configuration;

    using Narvalo.Web.Configuration;
    using Narvalo.Web.Properties;

    public static class AssetManager
    {
        // We use a volatile field to prevent any re-ordering inside EnsureIntialized_().
        private static volatile bool s_Initialized = false;
        private static readonly object s_Lock = new Object();

        private static AssetProviderBase s_Provider;
        private static AssetProviderCollection s_Providers;

        public static AssetProviderBase Provider
        {
            get
            {
                Contract.Ensures(Contract.Result<AssetProviderBase>() != null);

                EnsureInitialized_();
                return s_Provider;
            }
        }

        public static AssetProviderCollection Providers
        {
            get
            {
                Contract.Ensures(Contract.Result<AssetProviderCollection>() != null);

                EnsureInitialized_();
                return s_Providers;
            }
        }

        public static Uri ImageBase
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetImage(String.Empty);
            }
        }

        public static Uri ScriptBase
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetScript(String.Empty);
            }
        }

        public static Uri StyleBase
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetStyle(String.Empty);
            }
        }

        public static Uri GetImage(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetImage(relativePath);
        }

        public static Uri GetScript(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetScript(relativePath);
        }

        public static Uri GetStyle(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetStyle(relativePath);
        }

        private static void EnsureInitialized_()
        {
            if (!s_Initialized)
            {
                lock (s_Lock)
                {
                    if (!s_Initialized)
                    {
                        Initialize_();
                        s_Initialized = true;
                    }
                }
            }
        }

        private static void Initialize_()
        {
            var section = NarvaloWebConfigurationManager.AssetSection;

            // Initialize providers.
            var tmpProviders = new AssetProviderCollection();
            if (section.Providers != null)
            {
                ProvidersHelper.InstantiateProviders(section.Providers, tmpProviders, typeof(AssetProviderBase));
                tmpProviders.SetReadOnly();
            }

            s_Providers = tmpProviders;

            // Initialize default provider.
            if (section.DefaultProvider == null)
            {
                throw new ConfigurationErrorsException(
                    Strings_Web.AssetManager_DefaultProviderNotConfigured,
                    section.ElementInformation.Properties["providers"].Source,
                    section.ElementInformation.Properties["providers"].LineNumber);
            }

            s_Provider = s_Providers[section.DefaultProvider];

            if (s_Provider == null)
            {
                throw new ProviderException(Strings_Web.AssetManager_DefaultProviderNotFound);
            }
        }
    }
}
