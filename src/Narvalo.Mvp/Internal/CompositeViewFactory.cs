// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Narvalo.Mvp.Binder;

    internal static class CompositeViewFactory
    {
        // REVIEW: We use a concurrent dictionary as we expect to deal mostly with read operations
        // and to only do very few updates. Also note that, in most cases, the ICompositeViewTypeFactory
        // instance shall be unique during the lifetime of the application: PresenterBinder uses the 
        // static property CompositeViewTypeBuilder.Current.Factory.
        static readonly ConcurrentDictionary<RuntimeTypeHandle, Type> Cache_
            = new ConcurrentDictionary<RuntimeTypeHandle, Type>();

        public static IView CreateCompositeView(PresenterBinding binding)
        {
            var compositeViewType = GetCompositeViewType_(binding.ViewType);
            var view = (ICompositeView)Activator.CreateInstance(compositeViewType);

            foreach (var item in binding.Views) {
                view.Add(item);
            }

            return view;
        }

        static Type GetCompositeViewType_(Type viewType)
        {
            return Cache_.GetOrAdd(viewType.TypeHandle, _ => CreateCompositeViewType_(viewType));
        }

        static Type CreateCompositeViewType_(Type viewType)
        {
            /*
             * To support composite views, we dynamically emit a type which
             * takes multiple views, and exposes them as a single view of
             * the same interface. It's something like this:
             * 
public class TestViewComposite
    : CompositeView<ITestView>, ITestView
{
    public TestViewModel Model
    {
        get
        {
            return Views.First().Model;
        }
        set
        {
            foreach(var view in Views)
                view.Model = value;
        }
    }
    
    public event EventHandler Searching
    {
        add
        {
            foreach (var view in Views)
            {
                view.Searching += value;
            }
        }
        remove
        {
            foreach (var view in Views)
            {
                view.Searching -= value;
            }
        }
    }
}
             * 
             */

            ValidateViewType_(viewType);

            var builder = CompositeViewTypeBuilder.Create(viewType);

            var properties = FindProperties_(viewType);
            foreach (var propertyInfo in properties) {
                builder.AddProperty(propertyInfo);
            }

            var events = FindEvents_(viewType);
            foreach (var eventInfo in events) {
                builder.AddEvent(eventInfo);
            }

            return builder.Build();
        }

        static IEnumerable<EventInfo> FindEvents_(Type viewType)
        {
            return viewType.GetEvents()
                .Union(
                    viewType.GetInterfaces()
                        .SelectMany<Type, EventInfo>(FindEvents_)
                );
        }

        static IEnumerable<PropertyInfo> FindProperties_(Type viewType)
        {
            return viewType.GetProperties()
                .Union(
                    viewType.GetInterfaces().SelectMany<Type, PropertyInfo>(FindProperties_)
                )
                .Select(p => new
                {
                    PropertyInfo = p,
                    PropertyInfoFromCompositeViewBase = typeof(CompositeView<>).GetProperty(p.Name)
                })
                .Where(p =>
                    p.PropertyInfoFromCompositeViewBase == null
                    || (
                        p.PropertyInfoFromCompositeViewBase.GetGetMethod() == null
                        && p.PropertyInfoFromCompositeViewBase.GetSetMethod() == null)
                )
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