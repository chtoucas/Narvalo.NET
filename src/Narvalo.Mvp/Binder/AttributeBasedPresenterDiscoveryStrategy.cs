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
        readonly PresenterBindingAttributesProvider _attributesProvider
            = new CachedPresenterBindingAttributesProvider();

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(hosts, "hosts");
            Require.NotNull(views, "views");

            var hostAttributes = hosts.Except(views.OfType<Object>())
                .SelectMany(_ => _attributesProvider.GetComponent(_.GetType()))
                .ToList();

            var pendingViews = views.ToList();
            // REVIEW: I think it is simply not possible to go beyond pendingViews.Count() iterations.
            //var maxIterations = 10 * pendingViews.Count();
            var maxIterations = pendingViews.Count();
            var iterations = 0;

            while (pendingViews.Any()) {
                var view = pendingViews.First();
                var viewType = view.GetType();

                var bindings
                    = (from attr in
                           _attributesProvider.GetComponent(viewType).Concat(hostAttributes)
                       where attr.ViewType.IsAssignableFrom(viewType)
                       select new PresenterBinding(
                           attr.PresenterType,
                           attr.ViewType,
                           attr.BindingMode,
                           GetViewsToBind_(attr, view, viewType, pendingViews)
                       )).ToList();

                var boundViews = bindings.SelectMany(_ => _.Views)
                    // Concat with currently inspected view in case "bindings" is empty.
                    .Concat(new[] { view })
                    .Distinct()
                    .ToList();

                yield return new PresenterDiscoveryResult(boundViews, bindings);

                foreach (var item in boundViews) {
                    pendingViews.Remove(item);
                }

                if (iterations++ > maxIterations) {
                    throw new MvpException(
                        "The loop has executed too many times. An exit condition is failing and needs to be investigated.");
                }
            }
        }

        static IEnumerable<IView> GetViewsToBind_(
            PresenterBindingAttribute attribute,
            IView view,
            Type viewType,
            IEnumerable<IView> pendingViews)
        {
            __Trace.Write(
                "Found [PresenterBinding] attribute on {0} (presenter type: {1}, view type: {2}, binding mode: {3})",
                attribute.Origin.FullName,
                attribute.PresenterType.FullName,
                attribute.ViewType.FullName,
                attribute.BindingMode.ToString()
            );

            switch (attribute.BindingMode) {
                case PresenterBindingMode.Default:
                    return new[] { view };

                case PresenterBindingMode.SharedPresenter:
                    return pendingViews.Where(viewType.IsInstanceOfType);

                default:
                    throw new BindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported.",
                        attribute.BindingMode));
            }
        }
    }
}