// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
    using System.Diagnostics;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Resolvers;

    /// <summary>
    /// Convention based presenter discovery strategy.
    /// </summary>
    public sealed class /*Default*/PresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private readonly IPresenterTypeResolver _typeResolver;

        public PresenterDiscoveryStrategy(IPresenterTypeResolver typeResolver)
            : this(typeResolver, true)
        {
            Expect.NotNull(typeResolver);
        }

        public PresenterDiscoveryStrategy(
            IPresenterTypeResolver typeResolver,
            bool enableCache)
        {
            Require.NotNull(typeResolver, nameof(typeResolver));

            _typeResolver = enableCache
                 ? new CachedPresenterTypeResolver(typeResolver)
                 : typeResolver;
        }

        public PresenterDiscoveryResult FindBindings(IEnumerable<object> hosts, IEnumerable<IView> views)
        {
            Require.NotNull(views, nameof(views));

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBindingParameter>();

            foreach (var view in views)
            {
                if (view == null) { continue; }

                var viewType = view.GetType();
                var presenterType = _typeResolver.Resolve(viewType);

                if (presenterType != null)
                {
                    Trace.TraceInformation(
                        "[PresenterDiscoveryStrategy] Found presenter '{0}' for view '{1}'.",
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
                        "[PresenterDiscoveryStrategy] No presenter found for view '{0}'",
                        viewType.FullName);
                }
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_typeResolver != null);
        }

#endif
    }
}
