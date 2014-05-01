// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Generic;

    internal sealed class CachedPresenterBindingAttributesProvider
        : PresenterBindingAttributesProvider
    {
        readonly TypeKeyedProviderCache<IEnumerable<PresenterBindingAttribute>> _cache
           = new TypeKeyedProviderCache<IEnumerable<PresenterBindingAttribute>>();

        public override IEnumerable<PresenterBindingAttribute> GetComponent(Type input)
        {
            return _cache.GetOrAdd(input, base.GetComponent);
        }
    }
}
