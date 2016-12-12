// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Configuration
{
    using System;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    public sealed class Setter<TSource, T> where TSource : class
    {
        private readonly TSource _source;
        private readonly Action<T> _set;

        public Setter(TSource source, Action<T> set)
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(set, nameof(set));

            _source = source;
            _set = set;
        }

        public TSource Is(T value)
        {
            Warrant.NotNull<TSource>();

            _set.Invoke(value);

            return _source;
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_source != null);
            Contract.Invariant(_set != null);
        }

#endif
    }
}
