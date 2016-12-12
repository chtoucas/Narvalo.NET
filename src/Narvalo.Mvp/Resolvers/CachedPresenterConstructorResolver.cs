// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Reflection.Emit;

    public sealed class CachedPresenterConstructorResolver : IPresenterConstructorResolver
    {
        private readonly ResolverCache<Tuple<Type, Type>, string, DynamicMethod> _cache
           = new ResolverCache<Tuple<Type, Type>, string, DynamicMethod>(
                _ => String.Join(
                    "__:__",
                    new[]
                    {
                        _.Item1.AssemblyQualifiedName,
                        _.Item2.AssemblyQualifiedName
                    }));

        private readonly IPresenterConstructorResolver _inner;

        public CachedPresenterConstructorResolver(IPresenterConstructorResolver inner)
        {
            Require.NotNull(inner, nameof(inner));

            _inner = inner;
        }

        public DynamicMethod Resolve(Type presenterType, Type viewType)
        {
            return _cache.GetOrAdd(Tuple.Create(presenterType, viewType), _ => _inner.Resolve(_.Item1, _.Item2));
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_cache != null);
            Contract.Invariant(_inner != null);
        }

#endif
    }
}
