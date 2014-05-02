// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Resolvers
{
    using System;

    internal sealed class TypeKeyedResolverCache<TValue>
        : ResolverCache<Type, RuntimeTypeHandle, TValue>
    {
        public TypeKeyedResolverCache() : base(_ => _.TypeHandle) { }
    }
}