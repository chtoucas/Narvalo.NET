// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes",
           Justification = "Using three generic parameters does not seem that much!")]
    public class ResolverCache<TKey, TCacheKey, TValue> where TKey : class
    {
        private readonly ConcurrentDictionary<TCacheKey, TValue> _dictionary
           = new ConcurrentDictionary<TCacheKey, TValue>();

        private readonly Func<TKey, TCacheKey> _cacheKeyProvider;

        public ResolverCache(Func<TKey, TCacheKey> cacheKeyProvider)
        {
            Require.NotNull(cacheKeyProvider, nameof(cacheKeyProvider));

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));
            Expect.NotNull(key);

            TCacheKey innerKey = _cacheKeyProvider.Invoke(key);

            return _dictionary.GetOrAdd(innerKey, _ => valueFactory.Invoke(key));
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_cacheKeyProvider != null);
            Contract.Invariant(_dictionary != null);
        }

#endif
    }
}
