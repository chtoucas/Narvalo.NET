// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    public sealed class AttributedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private readonly IPresenterBindingAttributesResolver _attributesResolver;

        public AttributedPresenterDiscoveryStrategy()
            : this(new PresenterBindingAttributesResolver()) { }

        public AttributedPresenterDiscoveryStrategy(
            IPresenterBindingAttributesResolver attributesResolver)
            : this(attributesResolver, true)
        {
            Expect.NotNull(attributesResolver);
        }

        public AttributedPresenterDiscoveryStrategy(
            IPresenterBindingAttributesResolver attributesResolver,
            bool enableCache)
        {
            Require.NotNull(attributesResolver, "attributesResolver");

            _attributesResolver = enableCache
                 ? new CachedPresenterBindingAttributesResolver(attributesResolver)
                 : attributesResolver;
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

            while (pendingViews.Any())
            {
                var view = pendingViews.First();
                var viewType = view.GetType();

                var bindingsThisRound
                    = (from attr in
                           _attributesResolver.Resolve(viewType).Concat(hostAttributes)
                       where attr.ViewType.IsAssignableFrom(viewType)
                       select new PresenterBindingParameter(
                           attr.PresenterType,
                           attr.ViewType,
                           attr.BindingMode,
                           GetViewsToBind(attr, view, pendingViews))).ToList();

                bindings.AddRange(bindingsThisRound);

                var boundViewsThisRound = bindingsThisRound.SelectMany(_ => _.Views).ToList();

                boundViews.AddRange(boundViewsThisRound);

                // In case "boundViewsThisRound" is empty, we always add the currently
                // inspected view to the list of views to remove.
                pendingViews = pendingViews.Except(boundViewsThisRound.Concat(new[] { view }));
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }

        [SuppressMessage("Microsoft.Contracts", "Requires-7-138", Justification = "[Intentionally] Requires unreachable but CCCheck still proves no case is forgotten.")]
        private static IEnumerable<IView> GetViewsToBind(
            PresenterBindingAttribute attribute,
            IView view,
            IEnumerable<IView> pendingViews)
        {
            Trace.TraceInformation(
                "[AttributeBasedPresenterDiscoveryStrategy] Found presenter '{0}' for view '{1}', origin='{2}', binding mode='{3}'.",
                attribute.PresenterType.FullName,
                attribute.ViewType.FullName,
                attribute.Origin.FullName,
                attribute.BindingMode.ToString());

            switch (attribute.BindingMode)
            {
                case PresenterBindingMode.Default:
                    return new[] { view };

                case PresenterBindingMode.SharedPresenter:
                    return pendingViews.Where(attribute.ViewType.IsInstanceOfType);

                default:
                    throw Check.Unreachable("A case in a switch has been forgotten.");
            }
        }
    }
}