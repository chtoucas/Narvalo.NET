// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;

    internal sealed class InMemoryCache<TKey1, TKey2, TCacheKey, TValue>
    {
        readonly ConcurrentDictionary<TCacheKey, TValue> _cache
           = new ConcurrentDictionary<TCacheKey, TValue>();

        readonly Func<TKey1, TKey2, TCacheKey> _cacheKeyProvider;

        public InMemoryCache(Func<TKey1, TKey2, TCacheKey> cacheKeyProvider)
        {
            DebugCheck.NotNull(cacheKeyProvider);

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(
            TKey1 key1,
            TKey2 key2,
            Func<TKey1, TKey2, TValue> valueFactory)
        {
            DebugCheck.NotNull(key1);
            DebugCheck.NotNull(key2);
            DebugCheck.NotNull(valueFactory);

            var cacheKey = _cacheKeyProvider(key1, key2);

            return _cache.GetOrAdd(cacheKey, _ => valueFactory(key1, key2));
        }
    }
}
