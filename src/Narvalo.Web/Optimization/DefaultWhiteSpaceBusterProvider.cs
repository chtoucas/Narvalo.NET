// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Diagnostics.Contracts;

    public sealed class DefaultWhiteSpaceBusterProvider : IWhiteSpaceBusterProvider
    {
        private readonly IWhiteSpaceBuster _buster = new DefaultWhiteSpaceBuster();

        public IWhiteSpaceBuster Buster
        {
            get
            {
                Contract.Ensures(Contract.Result<IWhiteSpaceBuster>() != null);

                return _buster;
            }
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_buster != null);
        }

#endif
    }
}
