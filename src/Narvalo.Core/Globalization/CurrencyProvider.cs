// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;

    public sealed class CurrencyProvider
    {
        private static CurrencyProvider s_Instance = new CurrencyProvider();

        private Func<ICurrencyProvider> _factoryThunk = () => null;

        private ICurrencyProvider _current;

        internal CurrencyProvider() : this(null) { }

        internal CurrencyProvider(ICurrencyProvider provider)
        {
            InnerSetProvider(provider ?? new DefaultCurrencyProvider());
        }

        public static ICurrencyProvider Current
        {
            get { return s_Instance.InnerCurrent; }
        }

        internal ICurrencyProvider InnerCurrent
        {
            get { return _current ?? (_current = _factoryThunk()); }
        }

        public static void SetProvider(ICurrencyProvider provider)
        {
            s_Instance.InnerSetProvider(provider);
        }

        internal void InnerSetProvider(ICurrencyProvider provider)
        {
            Require.NotNull(provider, "provider");

            _factoryThunk = () => provider;
        }
    }
}
