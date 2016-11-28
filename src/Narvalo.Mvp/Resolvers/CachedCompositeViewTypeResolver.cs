// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif

    public sealed class CachedCompositeViewTypeResolver : ICompositeViewTypeResolver
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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_inner != null);
        }

#endif
    }
}
