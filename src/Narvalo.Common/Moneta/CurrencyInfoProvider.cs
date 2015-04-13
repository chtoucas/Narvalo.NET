// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("Gendarme.Rules.Naming", "AvoidTypeInterfaceInconsistencyRule")]
    public sealed class CurrencyInfoProvider
    {
        private static readonly CurrencyInfoProvider s_Instance = new CurrencyInfoProvider();

        private ICurrencyInfoProvider _current;

        public CurrencyInfoProvider() : this(null) { }

        public CurrencyInfoProvider(ICurrencyInfoProvider provider)
        {
            InnerSetProvider(provider ?? new InMemoryCurrencyInfoProvider());
        }

        public static ICurrencyInfoProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyInfoProvider>() != null);

                return s_Instance.InnerCurrent;
            }
        }

        public ICurrencyInfoProvider InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyInfoProvider>() != null);

                return _current;
            }
        }

        public static void SetProvider(ICurrencyInfoProvider provider)
        {
            Contract.Requires(provider != null);

            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(ICurrencyInfoProvider provider)
        {
            Require.NotNull(provider, "provider");

            _current = provider;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_current != null);
        }

#endif
    }
}
