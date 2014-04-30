// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;

    internal sealed class ReflectionCache<TValue>
        : KeyValueStore<Type, RuntimeTypeHandle, TValue>
    {
        public ReflectionCache() : base(_ => _.TypeHandle) { }
    }
}