namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration.Provider;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Justification = "ProviderCollection précède les classes génériques.")]
    public class AssetProviderCollection : ProviderCollection
    {
        public new AssetProviderBase this[string name]
        {
            get { return (AssetProviderBase)base[name]; }
        }

        public override void Add(ProviderBase provider)
        {
            Require.NotNull(provider, "provider");

            if (!(provider is AssetProviderBase)) {
                throw new ArgumentException(SR.AssetProviderCollection_InvalidProvider, "provider");
            }

            base.Add(provider);
        }

        public void CopyTo(AssetProviderBase[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
