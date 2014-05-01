// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Mvp.Internal;

    internal sealed class CachedViewInterfacesProvider : ViewInterfacesProvider
    {
        readonly TypeKeyedProviderCache<IEnumerable<Type>> _cache
           = new TypeKeyedProviderCache<IEnumerable<Type>>();

        public override IEnumerable<Type> GetComponent(Type input)
        {
            return _cache.GetOrAdd(input, base.GetComponent);
        }
    }
}
