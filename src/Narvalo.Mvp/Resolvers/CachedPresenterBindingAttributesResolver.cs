// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;

    public sealed partial class CachedPresenterBindingAttributesResolver
        : IPresenterBindingAttributesResolver
    {
        private readonly TypeKeyedResolverCache<IEnumerable<PresenterBindingAttribute>> _cache
           = new TypeKeyedResolverCache<IEnumerable<PresenterBindingAttribute>>();

        private readonly IPresenterBindingAttributesResolver _inner;

        public CachedPresenterBindingAttributesResolver(IPresenterBindingAttributesResolver inner)
        {
            Require.NotNull(inner, nameof(inner));

            _inner = inner;
        }

        public IEnumerable<PresenterBindingAttribute> Resolve(Type viewType)
        {
            return _cache.GetOrAdd(viewType, _inner.Resolve);
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System.Diagnostics.Contracts;

    public sealed partial class CachedPresenterBindingAttributesResolver
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_cache != null);
            Contract.Invariant(_inner != null);
        }
    }
}

#endif
