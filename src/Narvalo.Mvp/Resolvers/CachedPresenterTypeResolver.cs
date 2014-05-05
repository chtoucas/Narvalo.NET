// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public sealed class CachedPresenterTypeResolver : IPresenterTypeResolver
    {
        readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        readonly IPresenterTypeResolver _inner;

        public CachedPresenterTypeResolver(IPresenterTypeResolver inner)
        {
            Require.NotNull(inner, "inner");

            _inner = inner;
        }

        public Type Resolve(Type input)
        {
            return _cache.GetOrAdd(input, _inner.Resolve);
        }
    }
}
