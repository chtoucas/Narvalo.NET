// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.UI.Assets
{
    using System;
    using System.Configuration.Provider;
#if CONTRACTS_FULL && !CODE_ANALYSIS // [Intentionally] Using directive.
    using System.Diagnostics.Contracts;
#endif

    public abstract partial class AssetProviderBase : ProviderBase
    {
        protected AssetProviderBase() { }

        public abstract Uri GetFont(string relativePath);

        public abstract Uri GetImage(string relativePath);

        public abstract Uri GetScript(string relativePath);

        public abstract Uri GetStyle(string relativePath);
    }

#if CONTRACTS_FULL && !CODE_ANALYSIS // [Ignore] Contract Class and Object Invariants.

    [ContractClass(typeof(AssetProviderBaseContract))]
    public abstract partial class AssetProviderBase : ProviderBase { }

    [ContractClassFor(typeof(AssetProviderBase))]
    internal abstract class AssetProviderBaseContract : AssetProviderBase
    {
        public override Uri GetFont(string relativePath)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetImage(string relativePath)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetScript(string relativePath)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }

        public override Uri GetStyle(string relativePath)
        {
            Contract.Ensures(Contract.Result<Uri>() != null);

            return default(Uri);
        }
    }

#endif
}
