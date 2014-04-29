// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;

    internal  class InMemoryCache<TKey, TCacheKey, TValue>
    {
        readonly ConcurrentDictionary<TCacheKey, TValue> _dictionary
           = new ConcurrentDictionary<TCacheKey, TValue>();

        readonly Func<TKey, TCacheKey> _cacheKeyProvider;

        public InMemoryCache(Func<TKey, TCacheKey> cacheKeyProvider)
        {
            DebugCheck.NotNull(cacheKeyProvider);

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            DebugCheck.NotNull(key);
            DebugCheck.NotNull(valueFactory);

            var cacheKey = _cacheKeyProvider(key);

            return _dictionary.GetOrAdd(cacheKey, _ => valueFactory(key));
        }
    }
}
