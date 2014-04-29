// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;

    internal sealed class InMemoryCache<TValue>
    {
        readonly ConcurrentDictionary<RuntimeTypeHandle, TValue> _cache
           = new ConcurrentDictionary<RuntimeTypeHandle, TValue>();

        public TValue GetOrAdd(Type type, Func<Type, TValue> valueFactory)
        {
            DebugCheck.NotNull(type);
            DebugCheck.NotNull(valueFactory);

            return _cache.GetOrAdd(type.TypeHandle, _ => valueFactory(type));
        }
    }
}