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
        private static readonly object s_Lock = new Object();

        // We use a volatile field to prevent any re-ordering inside Initialize_().
        private static volatile bool s_Initialized;

        // Exception thrown during initialization.
        private static Exception s_InitializationException;

        private static AssetProvider s_Provider;
        private static AssetProviderCollection s_Providers;

        public static AssetProvider Provider
        {
            get
            {
                Contract.Ensures(Contract.Result<AssetProvider>() != null);

                Initialize();
                return s_Provider;
            }
        }

        public static AssetProviderCollection Providers
        {
            get
            {
                Contract.Ensures(Contract.Result<AssetProviderCollection>() != null);

                Initialize();
                return s_Providers;
            }
        }

        public static Uri FontsBaseUri
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetFontUri(String.Empty);
            }
        }

        public static Uri ImagesBaseUri
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetImageUri(String.Empty);
            }
        }

        public static Uri ScriptsBaseUri
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetScriptUri(String.Empty);
            }
        }

        public static Uri StylesBaseUri
        {
            get
            {
                Contract.Ensures(Contract.Result<Uri>() != null);

                return Provider.GetStyleUri(String.Empty);
            }
        }

        public static Uri GetFontUri(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetFontUri(relativePath);
        }

        public static Uri GetImageUri(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetImageUri(relativePath);
        }

        public static Uri GetScriptUri(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetScriptUri(relativePath);
        }

        public static Uri GetStyleUri(string relativePath)
        {
            Require.NotNullOrEmpty(relativePath, "relativePath");
            Contract.Ensures(Contract.Result<Uri>() != null);

            return Provider.GetStyleUri(relativePath);
        }

        // Internal-only method for testing.
        // We use temporary objects to achieve exception-neutral code.
        internal static void InitializeCore(AssetSection section)
        {
            Demand.NotNull(section);

            // Initialize the provider collection.
            var tmpProviders = new AssetProviderCollection();
            if (section.Providers != null)
            {
                ProvidersHelper.InstantiateProviders(section.Providers, tmpProviders, typeof(AssetProvider));
                tmpProviders.SetReadOnly();
            }

            s_Providers = tmpProviders;

            // Initialize the default provider.
            if (section.DefaultProvider == null)
            {
                throw new ProviderException(Strings.AssetManager_DefaultProviderNotConfigured);
            }

            var tmpProvider = s_Providers[section.DefaultProvider];
            if (tmpProvider == null)
            {
                // section.DefaultProvider is not null => propertyInfo != null.
                var propertyInfo
                    = section.ElementInformation
                        .Properties[AssetSection.DefaultProviderPropertyName];
                Contract.Assume(propertyInfo != null);

                throw new ConfigurationErrorsException(
                    Strings.AssetManager_DefaultProviderNotFound,
                    propertyInfo.Source,
                    propertyInfo.LineNumber);
            }

            s_Provider = tmpProvider;

            Contract.Assert(s_Providers != null);
            Contract.Assert(s_Provider != null);
        }

        // Internal-only method for testing.
        internal static void ResetCore()
        {
            s_Initialized = false;
        }

        private static void Initialize()
        {
            if (!s_Initialized)
            {
                lock (s_Lock)
                {
                    if (!s_Initialized)
                    {
                        try
                        {
                            var section = WebConfigurationManager.GetSection(AssetSection.SectionName) as AssetSection;
                            if (section == null)
                            {
                                InitializeDefault();
                            }
                            else
                            {
                                InitializeCore(section);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Keep the exception around.
                            // **Any** subsequent call to Initialize_() will throw this exception.
                            s_InitializationException = ex;

                            throw;
                        }
                        finally
                        {
                            // Even in case of failure, mark the type as initialized.
                            s_Initialized = true;
                        }
                    }
                }
            }

            if (s_InitializationException != null)
            {
                throw s_InitializationException;
            }
        }

        private static void InitializeDefault()
        {
            // Use the default provider. WARNING: Don't forget to initialize it too!
            var tmpProvider = new DefaultAssetProvider();
            tmpProvider.Initialize(null, null);

            var tmpProviders = new AssetProviderCollection();
            tmpProviders.Add(tmpProvider);
            tmpProviders.SetReadOnly();

            s_Providers = tmpProviders;
            s_Provider = tmpProvider;

            Contract.Assert(s_Providers != null);
            Contract.Assert(s_Provider != null);
        }
    }
}
