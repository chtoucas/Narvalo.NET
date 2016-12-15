// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    public sealed partial class CurrencyProvider
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
            get { Warrant.NotNull<CurrencyFactory>(); return s_Instance.InnerCurrent; }
        }

        public CurrencyFactory InnerCurrent
        {
            get { Warrant.NotNull<CurrencyFactory>(); return _factory; }
        }

        public static void SetFactory(CurrencyFactory factory)
        {
            Expect.NotNull(factory);

            s_Instance.InnerSetFactory(factory);
        }

        public void InnerSetFactory(CurrencyFactory factory)
        {
            Require.NotNull(factory, nameof(factory));

            _factory = factory;
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public sealed partial class CurrencyProvider
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_factory != null);
        }
    }
}

#endif
