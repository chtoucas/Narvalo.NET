namespace Narvalo.Web.UI.Assets 
{
    using System.Configuration.Provider;

    public abstract class AssetProviderBase : ProviderBase 
    {
        protected AssetProviderBase() { }

        public abstract VirtualAssetFile GetImage(string relativePath);

        public abstract VirtualAssetFile GetScript(string relativePath);

        public abstract VirtualAssetFile GetStyle(string relativePath);
    }
}
