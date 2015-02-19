// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public sealed class CurrencyProvider
    {
        static CurrencyProvider Instance_ = new CurrencyProvider();

        Func<ICurrencyProvider> _factoryThunk = () => null;

        ICurrencyProvider _current;

        internal CurrencyProvider() : this(null) { }

        internal CurrencyProvider(ICurrencyProvider provider)
        {
            InnerSetProvider(provider ?? new DefaultCurrencyProvider());
        }

        internal static ICurrencyProvider Current
        {
            get { return Instance_.InnerCurrent; }
        }

        internal ICurrencyProvider InnerCurrent
        {
            get { return _current ?? (_current = _factoryThunk()); }
        }

        public static void SetProvider(ICurrencyProvider provider)
        {
            Instance_.InnerSetProvider(provider);
        }

        internal void InnerSetProvider(ICurrencyProvider provider)
        {
            Require.NotNull(provider, "provider");

            _factoryThunk = () => provider;
        }
    }
}
