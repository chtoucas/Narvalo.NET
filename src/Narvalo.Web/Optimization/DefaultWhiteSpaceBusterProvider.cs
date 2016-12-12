// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    public sealed class DefaultWhiteSpaceBusterProvider : IWhiteSpaceBusterProvider
    {
        private readonly IWhiteSpaceBuster _buster = new DefaultWhiteSpaceBuster();

        public IWhiteSpaceBuster Buster
        {
            get
            {
                Warrant.NotNull<IWhiteSpaceBuster>();

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
