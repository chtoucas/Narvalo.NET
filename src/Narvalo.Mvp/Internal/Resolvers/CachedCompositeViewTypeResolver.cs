// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;

    internal sealed class CachedCompositeViewTypeResolver : CompositeViewTypeResolver
    {
        readonly TypeKeyedResolverCache<Type> _cache = new TypeKeyedResolverCache<Type>();

        public override Type Resolve(Type input)
        {
            return _cache.GetOrAdd(input, base.Resolve);
        }
    }
}
