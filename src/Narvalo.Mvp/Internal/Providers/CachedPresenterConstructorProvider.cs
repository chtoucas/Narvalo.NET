// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;
    using System.Reflection.Emit;

    internal sealed class CachedPresenterConstructorProvider
        : PresenterConstructorProvider
    {
        readonly ProviderCache<Tuple<Type, Type>, string, DynamicMethod> _cache
           = new ProviderCache<Tuple<Type, Type>, string, DynamicMethod>(_ => String.Join("__:__", new[]
            {
                _.Item1.AssemblyQualifiedName,
                _.Item2.AssemblyQualifiedName
            }));

        public override DynamicMethod GetComponent(Tuple<Type, Type> input)
        {
            return _cache.GetOrAdd(input, base.GetComponent);
        }
    }
}
