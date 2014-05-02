// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;
    using System.Reflection.Emit;

    internal sealed class CachedPresenterConstructorResolver
        : PresenterConstructorResolver
    {
        readonly ResolverCache<Tuple<Type, Type>, string, DynamicMethod> _cache
           = new ResolverCache<Tuple<Type, Type>, string, DynamicMethod>(_ => String.Join("__:__", new[]
            {
                _.Item1.AssemblyQualifiedName,
                _.Item2.AssemblyQualifiedName
            }));

        public override DynamicMethod Resolve(Tuple<Type, Type> input)
        {
            return _cache.GetOrAdd(input, base.Resolve);
        }
    }
}
