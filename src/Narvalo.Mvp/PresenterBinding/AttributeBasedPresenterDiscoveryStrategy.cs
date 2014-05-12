// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Narvalo;
    using Narvalo.Mvp.Resolvers;

    public sealed class AttributeBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IPresenterBindingAttributesResolver _attributesResolver;

        public AttributeBasedPresenterDiscoveryStrategy()
            : this(new PresenterBindingAttributesResolver()) { }

        public AttributeBasedPresenterDiscoveryStrategy(
            IPresenterBindingAttributesResolver attributesResolver)
            : this(attributesResolver, true) { }

        public AttributeBasedPresenterDiscoveryStrategy(
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

            while (pendingViews.Any()) {
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
                           GetViewsToBind_(attr, view, pendingViews)
                       )).ToList();

                bindings.AddRange(bindingsThisRound);

                var boundViewsThisRound = bindingsThisRound.SelectMany(_ => _.Views).ToList();

                boundViews.AddRange(boundViewsThisRound);

                // In case "boundViewsThisRound" is empty, we always add the currently 
                // inspected view to the list of views to remove.
                pendingViews = pendingViews.Except(boundViewsThisRound.Concat(new[] { view }));
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }

        static IEnumerable<IView> GetViewsToBind_(
            PresenterBindingAttribute attribute,
            IView view,
            IEnumerable<IView> pendingViews)
        {
            __Tracer.Info(
                typeof(AttributeBasedPresenterDiscoveryStrategy),
                @"Found presenter ""{0}"" for view ""{1}"", origin=""{2}"", binding mode=""{3}"".",
                attribute.PresenterType.FullName,
                attribute.ViewType.FullName,
                attribute.Origin.FullName,
                attribute.BindingMode.ToString()
            );

            switch (attribute.BindingMode) {
                case PresenterBindingMode.Default:
                    return new[] { view };

                case PresenterBindingMode.SharedPresenter:
                    return pendingViews.Where(attribute.ViewType.IsInstanceOfType);

                default:
                    throw new PresenterBindingException(String.Format(
                        CultureInfo.InvariantCulture,
                        "Binding mode {0} is not supported.",
                        attribute.BindingMode));
            }
        }
    }
}