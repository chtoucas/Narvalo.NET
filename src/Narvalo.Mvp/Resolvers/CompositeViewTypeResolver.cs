// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    public class /*Default*/CompositeViewTypeResolver : ICompositeViewTypeResolver
    {
        readonly CompositeViewModuleBuilder _moduleBuilder
              = new CompositeViewModuleBuilder("Narvalo.Mvp.CompositeViews");

        public virtual Type Resolve(Type input)
        {
            Require.NotNull(input, "input");

            __Tracer.Info(this, @"Attempting to resolve ""{0}"".", input.FullName);

            ValidateViewType_(input);

            var typeBuilder = new CompositeViewTypeBuilder(input, _moduleBuilder.DefineType(input));

            var properties = FindProperties_(input);
            foreach (var propertyInfo in properties) {
                typeBuilder.AddProperty(propertyInfo);
            }

            var events = FindEvents_(input);
            foreach (var eventInfo in events) {
                typeBuilder.AddEvent(eventInfo);
            }

            return typeBuilder.Build();
        }

        static IEnumerable<EventInfo> FindEvents_(Type viewType)
        {
            return viewType.GetEvents()
                .Union(
                    viewType.GetInterfaces()
                        .SelectMany<Type, EventInfo>(FindEvents_));
        }

        static IEnumerable<PropertyInfo> FindProperties_(Type viewType)
        {
            return viewType.GetProperties()
                .Union(
                    viewType.GetInterfaces().SelectMany<Type, PropertyInfo>(FindProperties_))
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

        static void ValidateViewType_(Type viewType)
        {
            if (!viewType.IsInterface) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "To be used with shared presenters, the view type must be an interface, but {0} was supplied instead.",
                    viewType.FullName));
            }

            if (!typeof(IView).IsAssignableFrom(viewType)) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "To be used with shared presenters, the view type must inherit from {0}. The supplied type ({1}) does not.",
                    typeof(IView).FullName,
                    viewType.FullName));
            }

            if (!viewType.IsPublic && !viewType.IsNestedPublic) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "To be used with shared presenters, the view type must be public. The supplied type ({0}) is not.",
                    viewType.FullName));
            }

            if (viewType.GetMethods().Where(_ => !_.IsSpecialName).Any()) {
                throw new ArgumentException(String.Format(
                    CultureInfo.InvariantCulture,
                    "To be used with shared presenters, the view type must not define public methods. The supplied type ({0}) is not.",
                    viewType.FullName));
            }
        }
    }
}
