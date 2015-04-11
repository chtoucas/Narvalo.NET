// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Diagnostics.Contracts;

    public sealed class CurrencyProvider
    {
        private static readonly CurrencyProvider s_Instance = new CurrencyProvider();

        private ICurrencyProvider _current;

        public CurrencyProvider() : this(null) { }

        public CurrencyProvider(ICurrencyProvider provider)
        {
            InnerSetProvider(provider ?? new InMemoryCurrencyProvider());
        }

        public static ICurrencyProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyProvider>() != null);

                return s_Instance.InnerCurrent;
            }
        }

        public ICurrencyProvider InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyProvider>() != null);

                return _current;
            }
        }

        public static void SetProvider(ICurrencyProvider provider)
        {
            Contract.Requires(provider != null);

            s_Instance.InnerSetProvider(provider);
        }

        public void InnerSetProvider(ICurrencyProvider provider)
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
