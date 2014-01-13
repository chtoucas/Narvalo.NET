namespace Narvalo.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Collections;
    using Narvalo.Presentation.Mvp;

    static class TypeExtensions
    {
        static readonly IDictionary<RuntimeTypeHandle, IEnumerable<Type>> ImplementationTypeToViewInterfacesCache_
           = new Dictionary<RuntimeTypeHandle, IEnumerable<Type>>();

        public static IEnumerable<Type> GetViewInterfaces(this Type implementationType)
        {
            // We use the type handle as the cache key because they're fast
            // to search against in dictionaries.
            var implementationTypeHandle = implementationType.TypeHandle;

            return ImplementationTypeToViewInterfacesCache_.GetOrCreateValue(implementationTypeHandle, () =>
                implementationType
                    .GetInterfaces()
                    .Where(typeof(IView).IsAssignableFrom)
                    .ToArray()
            );
        }
    }
}