// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public sealed class CachedCompositeViewTypeResolver : ICompositeViewTypeResolver
    {
        readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        readonly ICompositeViewTypeResolver _inner;

        public CachedCompositeViewTypeResolver(ICompositeViewTypeResolver inner)
        {
            Require.NotNull(inner, "inner");

            _inner = inner;
        }

        public Type Resolve(Type viewType)
        {
            return _cache.GetOrAdd(viewType, _inner.Resolve);
        }
    }
}
