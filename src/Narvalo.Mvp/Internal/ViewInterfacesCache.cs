// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp;

    internal static class ViewInterfacesCache
    {
        static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<Type>> Cache_
            = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<Type>>();

        public static IEnumerable<Type> GetViewInterfaces(Type viewType)
        {
            return Cache_.GetOrAdd(
                viewType.TypeHandle,
                _ => viewType.GetInterfaces().Where(typeof(IView).IsAssignableFrom).ToArray()
            );
        }
    }
}
