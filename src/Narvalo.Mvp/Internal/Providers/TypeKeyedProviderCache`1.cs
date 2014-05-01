// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal.Providers
{
    using System;

    internal sealed class TypeKeyedProviderCache<TValue>
        : ProviderCache<Type, RuntimeTypeHandle, TValue>
    {
        public TypeKeyedProviderCache() : base(_ => _.TypeHandle) { }
    }
}