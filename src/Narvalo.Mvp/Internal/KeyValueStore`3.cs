// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;

    internal class KeyValueStore<TKey, TInnerKey, TValue>
    {
        readonly ConcurrentDictionary<TInnerKey, TValue> _dictionary
           = new ConcurrentDictionary<TInnerKey, TValue>();

        readonly Func<TKey, TInnerKey> _innerKeyProvider;

        public KeyValueStore(Func<TKey, TInnerKey> innerKeyProvider)
        {
            DebugCheck.NotNull(innerKeyProvider);

            _innerKeyProvider = innerKeyProvider;
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            DebugCheck.NotNull(key);
            DebugCheck.NotNull(valueFactory);

            var innerKey = _innerKeyProvider(key);

            return _dictionary.GetOrAdd(innerKey, _ => valueFactory(key));
        }
    }
}
