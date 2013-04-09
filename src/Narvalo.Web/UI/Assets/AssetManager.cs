namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Web.Configuration;
    using Narvalo.Web.Configuration;

    public static class AssetManager
    {
        static AssetProviderBase Provider_;
        static AssetProviderCollection Providers_;

        static bool InitializedDefaultProvider_ = false;
        static bool InitializedProviders_ = false;
        static object Lock_ = new Object();

        public static AssetProviderBase Provider
        {
            get
            {
                EnsureInitialized();
                return Provider_;
            }
        }

        public static AssetProviderCollection Providers
        {
            get
            {
                EnsureInitialized();
                return Providers_;
            }
        }

        public static AssetFile GetImage(string relativePath)
        {
            Requires.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetImage(relativePath);
        }

        public static AssetFile GetScript(string relativePath)
        {
            Requires.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetScript(relativePath);
        }

        public static AssetFile GetStyle(string relativePath)
        {
            Requires.NotNullOrEmpty(relativePath, "relativePath");
            return Provider.GetStyle(relativePath);
        }

        private static void EnsureInitialized()
        {
            if (InitializedProviders_ && InitializedDefaultProvider_) {
                return;
            }

            lock (Lock_) {
                if (InitializedProviders_ && InitializedDefaultProvider_) {
                    return;
                }

                // FIXME: est-ce la bonne manière de faire ?
                var section = NarvaloWebConfigurationManager.GetSectionGroup().AssetSection;

                InitProviders(section);
                InitDefaultProvider(section);
            }
        }

        private static void InitProviders(AssetSection section)
        {
            if (InitializedProviders_) {
                return;
            }

            var tmpProviders = new AssetProviderCollection();
            if (section.Providers != null) {
                ProvidersHelper.InstantiateProviders(section.Providers, tmpProviders,
                                                     typeof(AssetProviderBase));
                tmpProviders.SetReadOnly();
            }

            Providers_ = tmpProviders;
            InitializedProviders_ = true;
        }

        private static void InitDefaultProvider(AssetSection section)
        {
            if (InitializedDefaultProvider_) {
                return;
            }

            if (section.DefaultProvider == null) {
                throw new ConfigurationErrorsException(
                    "The default AssetProvider was not specified.",
                    section.ElementInformation.Properties["providers"].Source,
                    section.ElementInformation.Properties["providers"].LineNumber);
            }

            Provider_ = Providers_[section.DefaultProvider];

            if (Provider_ == null) {
                throw new ProviderException("The default AssetProvider was not found.");
            }

            InitializedDefaultProvider_ = true;
        }
    }
}
