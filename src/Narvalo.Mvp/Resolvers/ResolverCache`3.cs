// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes",
           Justification = "Using three generic parameters does not seem that much!")]
    public class ResolverCache<TKey, TCacheKey, TValue>
    {
        readonly ConcurrentDictionary<TCacheKey, TValue> _dictionary
           = new ConcurrentDictionary<TCacheKey, TValue>();

        readonly Func<TKey, TCacheKey> _cacheKeyProvider;

        public ResolverCache(Func<TKey, TCacheKey> cacheKeyProvider)
        {
            Require.NotNull(cacheKeyProvider, "cacheKeyProvider");

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            Require.NotNull(valueFactory, "valueFactory");

            var innerKey = _cacheKeyProvider(key);

            return _dictionary.GetOrAdd(innerKey, _ => valueFactory(key));
        }
    }
}
