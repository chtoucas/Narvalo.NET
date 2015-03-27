// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Diagnostics.Contracts;
    using System.Web.Configuration;

    using Narvalo.Web.Configuration;
    using Narvalo.Web.Resources;

    // FIXME: The concurrency doulbe check is broken. Add volatile to the Initialized... fields?
    public static class AssetManager
    {
        private static readonly object s_Lock = new Object();

        private static AssetProviderBase s_Provider;
        private static AssetProviderCollection s_Providers;

        private static bool s_InitializedDefaultProvider = false;
        private static bool s_InitializedProviders = false;

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
            if (s_InitializedProviders && s_InitializedDefaultProvider)
            {
                return;
            }

            lock (s_Lock)
            {
                if (s_InitializedProviders && s_InitializedDefaultProvider)
                {
                    return;
                }

                var section = NarvaloWebConfigurationManager.AssetSection;

                InitProviders_(section);
                InitDefaultProvider_(section);
            }
        }

        private static void InitProviders_(AssetSection section)
        {
            if (s_InitializedProviders)
            {
                return;
            }

            var tmpProviders = new AssetProviderCollection();
            if (section.Providers != null)
            {
                ProvidersHelper.InstantiateProviders(section.Providers, tmpProviders, typeof(AssetProviderBase));
                tmpProviders.SetReadOnly();
            }

            s_Providers = tmpProviders;
            s_InitializedProviders = true;
        }

        private static void InitDefaultProvider_(AssetSection section)
        {
            if (s_InitializedDefaultProvider)
            {
                return;
            }

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

            s_InitializedDefaultProvider = true;
        }
    }
}
