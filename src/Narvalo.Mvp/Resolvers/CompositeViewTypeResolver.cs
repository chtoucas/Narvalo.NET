// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Linq;
    using System.Reflection;

    using Narvalo.Mvp.Properties;

    public class /*Default*/CompositeViewTypeResolver : ICompositeViewTypeResolver
    {
        private readonly CompositeViewModuleBuilder _moduleBuilder
            = new CompositeViewModuleBuilder("Narvalo.Mvp.CompositeViews");

        public Type Resolve(Type viewType)
        {
            Require.NotNull(viewType, nameof(viewType));

            Trace.TraceInformation("[CompositeViewTypeResolver] Attempting to resolve '{0}'", viewType.FullName);

            ValidateViewType(viewType);

            var typeBuilder = new CompositeViewTypeBuilder(viewType, _moduleBuilder.DefineType(viewType));

            var properties = FindProperties(viewType);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo == null) { continue; }

                typeBuilder.AddProperty(propertyInfo);
            }

            var events = FindEvents(viewType);
            foreach (var eventInfo in events)
            {
                if (eventInfo ==null) { continue; }

                typeBuilder.AddEvent(eventInfo);
            }

            return typeBuilder.Build();
        }

        internal static void ValidateViewType(Type viewType)
        {
            Require.NotNull(viewType, nameof(viewType));

            if (!viewType.IsInterface)
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeViewTypeResolver_ViewTypeIsNotInterface,
                        viewType.FullName),
                    nameof(viewType));
            }

            if (!typeof(IView).IsAssignableFrom(viewType))
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeViewTypeResolver_ViewTypeIsNotAssignable,
                        typeof(IView).FullName,
                        viewType.FullName),
                    nameof(viewType));
            }

            if (!viewType.IsPublic && !viewType.IsNestedPublic)
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeViewTypeResolver_ViewTypeIsNotPublic,
                        viewType.FullName),
                    nameof(viewType));
            }

            if (viewType.GetMethods().Where(_ => !_.IsSpecialName).Any())
            {
                throw new ArgumentException(Format.Current(
                        Strings.CompositeViewTypeResolver_ViewTypeContainsPublicMethods,
                        viewType.FullName),
                    nameof(viewType));
            }
        }

        private static IEnumerable<EventInfo> FindEvents(Type viewType)
        {
            Demand.NotNull(viewType);

            return viewType.GetEvents()
                .Union(
                    viewType.GetInterfaces()
                        .SelectMany<Type, EventInfo>(FindEvents));
        }

        private static IEnumerable<PropertyInfo> FindProperties(Type viewType)
        {
            Demand.NotNull(viewType);

            return viewType.GetProperties()
                .Union(
                    viewType.GetInterfaces().SelectMany<Type, PropertyInfo>(FindProperties))
                .Select(p => new
                {
                    PropertyInfo = p,
                    PropertyInfoFromCompositeViewBase = typeof(CompositeView<>).GetProperty(p.Name)
                })
                .Where(p =>
                    p.PropertyInfoFromCompositeViewBase == null
                    || (
                        p.PropertyInfoFromCompositeViewBase.GetGetMethod() == null
                        && p.PropertyInfoFromCompositeViewBase.GetSetMethod() == null))
                .Select(p => p.PropertyInfo);
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_moduleBuilder != null);
        }

#endif
    }
}
