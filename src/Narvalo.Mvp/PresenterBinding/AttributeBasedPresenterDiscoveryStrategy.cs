// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Internal;
    using Narvalo.Mvp.Resolvers;

    public sealed class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IPresenterBindingAttributesResolver _attributesResolver;

        public AttributeBasedPresenterDiscoveryStrategy()
            : this(
                new CachedPresenterBindingAttributesResolver(
                    new PresenterBindingAttributesResolver())) { }

        public AttributeBasedPresenterDiscoveryStrategy(IPresenterBindingAttributesResolver attributesResolver)
        {
            Require.NotNull(attributesResolver, "attributesResolver");

            _attributesResolver = attributesResolver;
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(hosts, "hosts");
            Require.NotNull(views, "views");

            var hostAttributes = hosts.Except(views.OfType<Object>())
                .SelectMany(_ => _attributesResolver.Resolve(_.GetType()))
                .ToList();

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBindingParameter>();

            var pendingViews = views;

            while (pendingViews.Any()) {
                var view = pendingViews.First();
                var viewType = view.GetType();

                // FIXME: Problem with ASP.Net dynamic view types.
                var bindingsThisRound
                    = (from attr in
                           _attributesResolver.Resolve(viewType).Concat(hostAttributes)
                       where attr.ViewType.IsAssignableFrom(viewType)
                       select new PresenterBindingParameter(
                           attr.PresenterType,
                           attr.ViewType,
                           attr.BindingMode,
                           GetViewsToBind_(attr, view, viewType, pendingViews)
                       )).ToList();

                bindings.AddRange(bindingsThisRound);

                var boundViewsThisRound = bindingsThisRound.SelectMany(_ => _.Views).ToList();

                boundViews.AddRange(boundViewsThisRound);

                // In case "boundViewsThisRound" is empty, we always add the currently inspected view 
                // to the list of views to remove.
                pendingViews = pendingViews.Except(boundViewsThisRound.Concat(new[] { view }));
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }

        static IEnumerable<IView> GetViewsToBind_(
            PresenterBindingAttribute attribute,
            IView view,
            Type viewType,
            IEnumerable<IView> pendingViews)
        {
            __Trace.Write(
                "[AttributeBasedPresenterDiscoveryStrategy] Found presenter type: {1}, view type: {2}, binding mode: {3}.",
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
                    throw new PresenterBindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported.",
                        attribute.BindingMode));
            }
        }
    }
}