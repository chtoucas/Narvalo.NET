// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public sealed partial class CachedCompositeViewTypeResolver : ICompositeViewTypeResolver
    {
        private readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        private readonly ICompositeViewTypeResolver _inner;

        public CachedCompositeViewTypeResolver(ICompositeViewTypeResolver inner)
        {
            Require.NotNull(inner, nameof(inner));

            _inner = inner;
        }

        public Type Resolve(Type viewType)
        {
            return _cache.GetOrAdd(viewType, _inner.Resolve);
        }
    }
}
