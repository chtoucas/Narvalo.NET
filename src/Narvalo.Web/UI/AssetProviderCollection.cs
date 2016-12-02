// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI
{
    using System;
    using System.Configuration.Provider;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Web.Properties;

    [SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface",
        Justification = "[Intentionally] ProviderCollection existed before generics even did.")]
    public sealed class AssetProviderCollection : ProviderCollection
    {
        public new AssetProvider this[string name]
        {
            get { return (AssetProvider)base[name]; }
        }

        public override void Add(ProviderBase provider)
        {
            Require.NotNull(provider, nameof(provider));

            if (!(provider is AssetProvider))
            {
                throw new ArgumentException(Strings.AssetProviderCollection_InvalidProvider, nameof(provider));
            }

            base.Add(provider);
        }

        public void CopyTo(AssetProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
