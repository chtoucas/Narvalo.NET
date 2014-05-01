// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Concurrent;

    internal class ProviderCache<TKey, TCacheKey, TValue>
    {
        readonly ConcurrentDictionary<TCacheKey, TValue> _dictionary
           = new ConcurrentDictionary<TCacheKey, TValue>();

        readonly Func<TKey, TCacheKey> _cacheKeyProvider;

        public ProviderCache(Func<TKey, TCacheKey> cacheKeyProvider)
        {
            DebugCheck.NotNull(cacheKeyProvider);

            _cacheKeyProvider = cacheKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            DebugCheck.NotNull(key);
            DebugCheck.NotNull(valueFactory);

            var innerKey = _cacheKeyProvider(key);

            return _dictionary.GetOrAdd(innerKey, _ => valueFactory(key));
        }
    }
}
