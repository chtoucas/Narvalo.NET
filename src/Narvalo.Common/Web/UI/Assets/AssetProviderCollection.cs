namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration.Provider;

    public class AssetProviderCollection : ProviderCollection
    {
        public new AssetProviderBase this[string name]
        {
            get { return (AssetProviderBase)base[name]; }
        }

        public override void Add(ProviderBase provider)
        {
            Requires.NotNull(provider, "provider");

            if (!(provider is AssetProviderBase)) {
                throw Failure.Argument("provider", SR.AssetProviderCollection_InvalidProvider);
            }

            base.Add(provider);
        }

        public void CopyTo(AssetProviderBase[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
