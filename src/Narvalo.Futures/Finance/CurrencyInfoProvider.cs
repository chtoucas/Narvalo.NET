// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public sealed class CurrencyInfoProvider
    {
        private static readonly CurrencyInfoProvider s_Instance = new CurrencyInfoProvider();

        private ICurrencyInfoProvider _current;

        public CurrencyInfoProvider() : this(null) { }

        public CurrencyInfoProvider(ICurrencyInfoProvider provider)
        {
            InnerSetProvider(provider ?? new SnvCurrencyInfoProvider());
        }

        public static ICurrencyInfoProvider Current
        {
            get { Warrant.NotNull<ICurrencyInfoProvider>(); return s_Instance.InnerCurrent; }
        }

        public ICurrencyInfoProvider InnerCurrent
        {
            get { Warrant.NotNull<ICurrencyInfoProvider>(); return _current; }
        }

        public static void SetProvider(ICurrencyInfoProvider provider)
        {
            Expect.NotNull(provider);

            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(ICurrencyInfoProvider provider)
        {
            Require.NotNull(provider, nameof(provider));

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
