// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;
    using System.Collections.Generic;

    internal sealed class CachedPresenterBindingAttributesResolver
        : PresenterBindingAttributesResolver
    {
        readonly TypeKeyedResolverCache<IEnumerable<PresenterBindingAttribute>> _cache
           = new TypeKeyedResolverCache<IEnumerable<PresenterBindingAttribute>>();

        public override IEnumerable<PresenterBindingAttribute> Resolve(Type input)
        {
            return _cache.GetOrAdd(input, base.Resolve);
        }
    }
}
