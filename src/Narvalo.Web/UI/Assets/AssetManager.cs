// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Web.Configuration;
    using Narvalo.Web.Configuration;

    public static class AssetManager
    {
        static readonly object s_Lock = new Object();

        static AssetProviderBase s_Provider;
        static AssetProviderCollection s_Providers;

        static bool s_InitializedDefaultProvider = false;
        static bool s_InitializedProviders = false;

        public static AssetProviderBase Provider
        {
            get
            {
                EnsureInitialized_();
                return s_Provider;
            }
        }

        public static AssetProviderCollection Providers
        {
            get
            {
                EnsureInitialized_();
                return s_Providers;
            }
        }

        public static Uri ImageBase { get { return Provider.GetImage(String.Empty); } }

        public static Uri ScriptBase { get { return Provider.GetScript(String.Empty); } }

        public static Uri StyleBase { get { return Provider.GetStyle(String.Empty); } }

        public static Uri GetImage(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetImage(relativePath);
        }

        public static Uri GetScript(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetScript(relativePath);
        }

        public static Uri GetStyle(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetStyle(relativePath);
        }

        static void EnsureInitialized_()
        {
            if (s_InitializedProviders && s_InitializedDefaultProvider) {
                return;
            }

            lock (s_Lock) {
                if (s_InitializedProviders && s_InitializedDefaultProvider) {
                    return;
                }

                var section = NarvaloWebConfigurationManager.AssetSection;

                InitProviders_(section);
                InitDefaultProvider_(section);
            }
        }

        static void InitProviders_(AssetSection section)
        {
            if (s_InitializedProviders) {
                return;
            }

            var tmpProviders = new AssetProviderCollection();
            if (section.Providers != null) {
                ProvidersHelper.InstantiateProviders(section.Providers, tmpProviders, typeof(AssetProviderBase));
                tmpProviders.SetReadOnly();
            }

            s_Providers = tmpProviders;
            s_InitializedProviders = true;
        }

        static void InitDefaultProvider_(AssetSection section)
        {
            if (s_InitializedDefaultProvider) {
                return;
            }

            if (section.DefaultProvider == null) {
                throw new ConfigurationErrorsException(
                    Strings_Web.AssetManager_DefaultProviderNotConfigured,
                    section.ElementInformation.Properties["providers"].Source,
                    section.ElementInformation.Properties["providers"].LineNumber);
            }

            s_Provider = s_Providers[section.DefaultProvider];

            if (s_Provider == null) {
                throw new ProviderException(Strings_Web.AssetManager_DefaultProviderNotFound);
            }

            s_InitializedDefaultProvider = true;
        }
    }
}
