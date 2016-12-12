// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;

    public sealed partial class Appender<TSource, T> where TSource : class
    {
        private readonly TSource _source;
        private readonly Action<T> _append;

        public Appender(TSource source, Action<T> append)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(append, nameof(append));

            _source = source;
            _append = append;
        }

        public TSource With(params T[] values)
        {
            Require.NotNull(values, nameof(values));
            Warrant.NotNull<TSource>();

            foreach (var value in values)
            {
                if (value == null) { continue; }

                _append(value);
            }

            return _source;
        }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Configuration
{
    using System.Diagnostics.Contracts;

    public sealed partial class Appender<TSource, T>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_source != null);
            Contract.Invariant(_append != null);
        }
    }
}

#endif
