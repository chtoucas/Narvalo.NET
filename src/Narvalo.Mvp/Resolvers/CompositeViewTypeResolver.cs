// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

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
                typeBuilder.AddProperty(propertyInfo);
            }

            var events = FindEvents(viewType);
            foreach (var eventInfo in events)
            {
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
                        "To be used with shared presenters, the view type must be an interface, but {0} was supplied instead.",
                        viewType.FullName),
                    nameof(viewType));
            }

            if (!typeof(IView).IsAssignableFrom(viewType))
            {
                throw new ArgumentException(Format.Current(
                        "To be used with shared presenters, the view type must inherit from {0}. The supplied type ({1}) does not.",
                        typeof(IView).FullName,
                        viewType.FullName),
                    nameof(viewType));
            }

            if (!viewType.IsPublic && !viewType.IsNestedPublic)
            {
                throw new ArgumentException(Format.Current(
                        "To be used with shared presenters, the view type must be public. The supplied type ({0}) is not.",
                        viewType.FullName),
                    nameof(viewType));
            }

            if (viewType.GetMethods().Where(_ => !_.IsSpecialName).Any())
            {
                throw new ArgumentException(Format.Current(
                        "To be used with shared presenters, the view type must not define public methods. The supplied type ({0}) is not.",
                        viewType.FullName),
                    nameof(viewType));
            }
        }

        private static IEnumerable<EventInfo> FindEvents(Type viewType)
        {
            return viewType.GetEvents()
                .Union(
                    viewType.GetInterfaces()
                        .SelectMany<Type, EventInfo>(FindEvents));
        }

        private static IEnumerable<PropertyInfo> FindProperties(Type viewType)
        {
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
    }
}
