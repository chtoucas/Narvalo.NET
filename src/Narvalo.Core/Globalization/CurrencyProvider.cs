// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;
    using System.Diagnostics.Contracts;

    public sealed class CurrencyProvider
    {
        private static CurrencyProvider s_Instance = new CurrencyProvider();

        private Func<ICurrencyProvider> _factoryThunk; // = () => null;

        private ICurrencyProvider _current;

        internal CurrencyProvider() : this(null) { }

        internal CurrencyProvider(ICurrencyProvider provider)
        {
            InnerSetProvider(provider ?? new DefaultCurrencyProvider());
        }

        public static ICurrencyProvider Current
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyProvider>() != null);

                return s_Instance.InnerCurrent;
            }
        }

        internal ICurrencyProvider InnerCurrent
        {
            get
            {
                Contract.Ensures(Contract.Result<ICurrencyProvider>() != null);

                // NB: From the way _factoryThunk is built, it should be clear
                // that the result of its invocation is never null.
                return _current ?? (_current = _factoryThunk().AssumeNotNull());
            }
        }

        public static void SetProvider(ICurrencyProvider provider)
        {
            Contract.Requires(provider != null);

            s_Instance.InnerSetProvider(provider);
        }

        internal void InnerSetProvider(ICurrencyProvider provider)
        {
            Require.NotNull(provider, "provider");

            _factoryThunk = () => provider;
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_factoryThunk != null);
        }

#endif
    }
}
