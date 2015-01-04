// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection.Emit;

    public sealed class CachedPresenterConstructorResolver : IPresenterConstructorResolver
    {
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines",
            Justification = "Inline initialization of a field.")]
        readonly ResolverCache<Tuple<Type, Type>, string, DynamicMethod> _cache
           = new ResolverCache<Tuple<Type, Type>, string, DynamicMethod>(
                _ => String.Join(
                    "__:__",
                    new[] 
                    {
                        _.Item1.AssemblyQualifiedName,
                        _.Item2.AssemblyQualifiedName
                    }));

        readonly IPresenterConstructorResolver _inner;

        public CachedPresenterConstructorResolver(IPresenterConstructorResolver inner)
        {
            Require.NotNull(inner, "inner");

            _inner = inner;
        }

        public DynamicMethod Resolve(Type presenterType, Type viewType)
        {
            return _cache.GetOrAdd(Tuple.Create(presenterType, viewType), _ => _inner.Resolve(_.Item1, _.Item2));
        }
    }
}
