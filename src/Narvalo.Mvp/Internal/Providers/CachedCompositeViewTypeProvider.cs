// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;

    internal sealed class CachedCompositeViewTypeProvider : CompositeViewTypeProvider
    {
        readonly TypeKeyedProviderCache<Type> _cache = new TypeKeyedProviderCache<Type>();

        public override Type GetComponent(Type input)
        {
            return _cache.GetOrAdd(input, base.GetComponent);
        }
    }
}
