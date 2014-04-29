// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Internal;

    public sealed class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly InMemoryCache<IEnumerable<PresenterBindingAttribute>> _attributesCache
           = new InMemoryCache<IEnumerable<PresenterBindingAttribute>>();

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(hosts, "hosts");
            Require.NotNull(views, "views");

            var pendingViews = views.ToList();

            var iterations = 0;
            var maxIterations = 10 * pendingViews.Count();

            while (pendingViews.Any()) {
                var bindings = new List<PresenterBinding>();

                var view = pendingViews.First();
                var viewType = view.GetType();

                var viewAttributes = GetAttributes_(viewType)
                    .Where(_ => _.ViewType.IsAssignableFrom(viewType))
                    .OrderBy(_ => _.PresenterType.Name);

                foreach (var attribute in viewAttributes) {
                    var viewsToBind = GetViewsToBind_(
                        pendingViews, view, viewType, attribute);

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        viewsToBind
                    ));
                }

                var hostAttributes = hosts
                    .Except(views.OfType<Object>())
                    .SelectMany(h => GetAttributes_(h.GetType()))
                    .Where(_ => _.ViewType.IsAssignableFrom(viewType))
                    .OrderBy(_ => _.PresenterType.Name);

                foreach (var attribute in hostAttributes) {
                    var viewsToBind = GetViewsToBind_(
                        pendingViews, view, viewType, attribute);

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        viewsToBind
                    ));
                }

                var boundViews
                    = bindings.SelectMany(b => b.Views).Concat(new[] { view }).Distinct();

                yield return new PresenterDiscoveryResult(boundViews, bindings);

                // FIXME: It fail when boundViews has been modified outside? Where does it occur? 
                // Temporary fix: call ToList().
                foreach (var item in boundViews.ToList()) {
                    pendingViews.Remove(item);
                }

                if (iterations++ > maxIterations) {
                    throw new ApplicationException(
                        "The loop has executed too many times. An exit condition is failing and needs to be investigated.");
                }
            }
        }

        IEnumerable<PresenterBindingAttribute> GetAttributes_(Type sourceType)
        {
            return _attributesCache.GetOrAdd(sourceType, CreateAttributes_);
        }

        static IEnumerable<IView> GetViewsToBind_(
            IEnumerable<IView> pendingViews,
            IView view,
            Type viewType,
            PresenterBindingAttribute attribute)
        {
            IEnumerable<IView> viewsToBind;

            switch (attribute.BindingMode) {
                case PresenterBindingMode.Default:
                    viewsToBind = new[] { view };
                    break;

                case PresenterBindingMode.SharedPresenter:
                    viewsToBind = pendingViews.Where(viewType.IsInstanceOfType);
                    break;

                default:
                    throw new NotSupportedException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported",
                        attribute.BindingMode));
            }

            return viewsToBind;
        }

        static IEnumerable<PresenterBindingAttribute> CreateAttributes_(Type sourceType)
        {
            var attributes = sourceType
                .GetCustomAttributes(typeof(PresenterBindingAttribute), true /* inherit */)
                .OfType<PresenterBindingAttribute>()
                .ToArray();

            if (attributes.Any(a =>
                    a.BindingMode == PresenterBindingMode.SharedPresenter && a.ViewType == null
                )) {
                throw new NotSupportedException(string.Format(
                    CultureInfo.InvariantCulture,
                    "When a {1} is applied with BindingMode={2}, the ViewType must be explicitly specified. One of the bindings on {0} violates this restriction.",
                    sourceType.FullName,
                    typeof(PresenterBindingAttribute).Name,
                    Enum.GetName(typeof(PresenterBindingMode), PresenterBindingMode.SharedPresenter)
                ));
            }

            attributes = attributes
                .Select(pba =>
                    new PresenterBindingAttribute(pba.PresenterType)
                    {
                        ViewType = pba.ViewType ?? sourceType,
                        BindingMode = pba.BindingMode
                    })
                .ToArray();

            return attributes;
        }
    }
}