namespace Narvalo.Web.UI.Assets 
{
    using System.Configuration.Provider;

    public abstract class AssetProviderBase : ProviderBase 
    {
        protected AssetProviderBase() { }

        public abstract AssetFile GetImage(string relativePath);

        public abstract AssetFile GetScript(string relativePath);

        public abstract AssetFile GetStyle(string relativePath);
    }
}
