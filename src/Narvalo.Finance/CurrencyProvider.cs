// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using static System.Diagnostics.Contracts.Contract;

    public sealed class CurrencyProvider
    {
        private static readonly CurrencyProvider s_Instance = new CurrencyProvider();

        private CurrencyFactory _factory;

        public CurrencyProvider() : this(null) { }

        public CurrencyProvider(CurrencyFactory factory)
        {
            InnerSetFactory(factory ?? new DefaultCurrencyFactory());
        }

        public static CurrencyFactory Current
        {
            get
            {
                Ensures(Result<CurrencyFactory>() != null);

                return s_Instance.InnerCurrent;
            }
        }

        public CurrencyFactory InnerCurrent
        {
            get
            {
                Ensures(Result<CurrencyFactory>() != null);

                return _factory;
            }
        }

        public static void SetFactory(CurrencyFactory factory)
        {
            Demand.NotNull(factory);

            s_Instance.InnerSetFactory(factory);
        }

        public void InnerSetFactory(CurrencyFactory factory)
        {
            Require.NotNull(factory, nameof(factory));

            _factory = factory;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [System.Diagnostics.Contracts.ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Invariant(_factory != null);
        }

#endif
    }
}
