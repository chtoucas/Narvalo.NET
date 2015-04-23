// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Web.Optimization
{
    using System.Diagnostics.Contracts;

    public sealed class WhiteSpaceBusterProvider
    {
        private static readonly WhiteSpaceBusterProvider s_Instance = new WhiteSpaceBusterProvider();

        private IWhiteSpaceBusterProvider _current;

        public WhiteSpaceBusterProvider() : this(null) { }

        public WhiteSpaceBusterProvider(IWhiteSpaceBusterProvider provider)
        {
            InnerSetProvider(provider ?? new DefaultWhiteSpaceBusterProvider());
        }

        public static IWhiteSpaceBusterProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<IWhiteSpaceBusterProvider>() != null);

                return s_Instance.InnerCurrent;
            }
        }

        public IWhiteSpaceBusterProvider InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<IWhiteSpaceBusterProvider>() != null);

                return _current;
            }
        }

        public static void SetProvider(IWhiteSpaceBusterProvider provider)
        {
            Contract.Requires(provider != null);

            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(IWhiteSpaceBusterProvider provider)
        {
            Require.NotNull(provider, "provider");

            _current = provider;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_current != null);
        }

#endif
    }
}
