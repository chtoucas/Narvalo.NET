// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Internal;
    using Narvalo.Mvp.Internal.Providers;

    public sealed class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly PresenterBindingAttributesProvider _presenterBindingAttributesProvider 
            = new CachedPresenterBindingAttributesProvider();

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(hosts, "hosts");
            Require.NotNull(views, "views");

            var result = new List<PresenterDiscoveryResult>();

            var pendingViews = views.ToList();

            var iterations = 0;
            var maxIterations = 10 * pendingViews.Count();

            while (pendingViews.Any()) {
                var bindings = new List<PresenterBinding>();

                var view = pendingViews.First();
                var viewType = view.GetType();

                var viewAttributes = GetAttributes_(viewType)
                    .Where(_ => _.ViewType.IsAssignableFrom(viewType));
                //.OrderBy(_ => _.PresenterType.Name);

                foreach (var attribute in viewAttributes) {
                    var viewsToBind = GetViewsToBind_(pendingViews, view, viewType, attribute);

                    __Trace.Write(
                        "Found [PresenterBinding] attribute on view {0} (presenter type: {1}, view type: {2}, binding mode: {3})",
                        viewType.FullName,
                        attribute.PresenterType.FullName,
                        attribute.ViewType.FullName,
                        attribute.BindingMode.ToString()
                    );

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        viewsToBind
                    ));
                }

                var hostAttributes = hosts
                    .Except(views.OfType<Object>())
                    .SelectMany(h => GetAttributes_(h.GetType())
                        .Select(a => new { Host = h, Attribute = a }))
                    .Where(_ => _.Attribute.ViewType.IsAssignableFrom(viewType));
                //.OrderBy(_ => _.Attribute.PresenterType.Name);

                foreach (var hostAttribute in hostAttributes) {
                    var attribute = hostAttribute.Attribute;

                    var viewsToBind = GetViewsToBind_(
                        pendingViews, view, viewType, hostAttribute.Attribute);

                    __Trace.Write(
                        "Found [PresenterBinding] attribute on host {0} (presenter type: {1}, view type: {2}, binding mode: {3})",
                        hostAttribute.Host.GetType().FullName,
                        attribute.PresenterType.FullName,
                        attribute.ViewType.FullName,
                        attribute.BindingMode.ToString()
                    );

                    bindings.Add(new PresenterBinding(
                        attribute.PresenterType,
                        attribute.ViewType,
                        attribute.BindingMode,
                        viewsToBind
                    ));
                }

                var boundViews
                    = bindings.SelectMany(b => b.Views).Concat(new[] { view }).Distinct();

                result.Add(new PresenterDiscoveryResult(boundViews, bindings));

                // FIXME: It fails when "boundViews" has been modified outside.
                // Temporary fix: Call ToList().
                foreach (var item in boundViews.ToList()) {
                    pendingViews.Remove(item);
                }

                if (iterations++ > maxIterations) {
                    throw new BindingException(
                        "The loop has executed too many times. An exit condition is failing and needs to be investigated.");
                }
            }

            return result;
        }

        IEnumerable<PresenterBindingAttribute> GetAttributes_(Type sourceType)
        {
            return _presenterBindingAttributesProvider.GetComponent(sourceType);
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
                    throw new BindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported.",
                        attribute.BindingMode));
            }

            return viewsToBind;
        }
    }
}