// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Resolvers;

    public sealed class ConventionBasedPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private readonly IPresenterTypeResolver _typeResolver;

        public ConventionBasedPresenterDiscoveryStrategy(IPresenterTypeResolver typeResolver)
            : this(typeResolver, true)
        {
            Expect.NotNull(typeResolver);
        }

        public ConventionBasedPresenterDiscoveryStrategy(
            IPresenterTypeResolver typeResolver,
            bool enableCache)
        {
            Require.NotNull(typeResolver, "typeResolver");

            _typeResolver = enableCache
                 ? new CachedPresenterTypeResolver(typeResolver)
                 : typeResolver;
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBindingParameter>();

            foreach (var view in views)
            {
                var viewType = view.GetType();
                var presenterType = _typeResolver.Resolve(viewType);

                if (presenterType != null)
                {
                    Trace.TraceInformation(
                        "[ConventionBasedPresenterDiscoveryStrategy] Found presenter '{0}' for view '{1}'.",
                        presenterType.FullName,
                        viewType.FullName);

                    var binding = new PresenterBindingParameter(
                        presenterType,
                        viewType,
                        PresenterBindingMode.Default,
                        new[] { view });

                    bindings.Add(binding);
                    boundViews.Add(view);
                }
                else
                {
                    Trace.TraceInformation(
                        "[ConventionBasedPresenterDiscoveryStrategy] No presenter found for view '{0}'",
                        viewType.FullName);
                }
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }
    }
}
