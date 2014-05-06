// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal;
    using Narvalo.Mvp.Resolvers;

    public sealed class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IPresenterTypeResolver _typeResolver;

        public ConventionBasedPresenterDiscoveryStrategy(
            IBuildManager buildManager,
            IEnumerable<string> defaultNamespaces,
            string[] viewSuffixes,
            string[] presenterNameTemplates)
            : this(
                new CachedPresenterTypeResolver(
                    new PresenterTypeResolver(
                        buildManager,
                        defaultNamespaces,
                        viewSuffixes,
                        presenterNameTemplates))) { }

        public ConventionBasedPresenterDiscoveryStrategy(IPresenterTypeResolver typeResolver)
        {
            Require.NotNull(typeResolver, "typeResolver");

            _typeResolver = typeResolver;
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBindingParameter>();

            foreach (var view in views) {
                var viewType = view.GetType();
                var presenterType = _typeResolver.Resolve(viewType);

                if (presenterType != null) {
                    __Trace.Write(
                        "[ConventionBasedPresenterDiscoveryStrategy] Found presenter type: {0} for view type: {1}.",
                        presenterType.FullName,
                        viewType.FullName
                    );

                    var binding = new PresenterBindingParameter(
                        presenterType,
                        viewType,
                        PresenterBindingMode.Default,
                        new[] { view });

                    bindings.Add(binding);
                    boundViews.Add(view);
                }
                else {
                    __Trace.Write(
                        "[ConventionBasedPresenterDiscoveryStrategy] No presenter type found for view type: {0}.",
                        viewType.FullName
                    );
                }
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }
    }
}
