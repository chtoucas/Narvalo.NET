// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;
    using Narvalo;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Internal.Providers;

    public sealed class DefaultPresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        static readonly IEnumerable<string> ViewInstanceSuffixes_ = new[] 
        {
            "UserControl",
            "Control",
            "View",
            "Form",
        };

        static readonly IEnumerable<string> CandidatePresenterNames_ = new[]
        {
            "{namespace}.Presenters.{presenter}",
            "{namespace}.{presenter}",
        };

        readonly PresenterTypeProvider _typeProvider;

        public DefaultPresenterDiscoveryStrategy()
            : this(ViewInstanceSuffixes_, CandidatePresenterNames_) { }

        public DefaultPresenterDiscoveryStrategy(
            IEnumerable<string> viewInstanceSuffixes,
            IEnumerable<string> candidatePresenterNames)
        {
            _typeProvider = new CachedPresenterTypeProvider(
                new CachedViewInterfacesProvider(),
                viewInstanceSuffixes,
                candidatePresenterNames);
        }

        public PresenterDiscoveryResult FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            var boundViews = new List<IView>();
            var bindings = new List<PresenterBinding>();

            foreach (var view in views) {
                var viewType = view.GetType();
                var presenterType = _typeProvider.GetComponent(viewType);

                if (presenterType != null) {
                    var binding = new PresenterBinding(
                        presenterType,
                        viewType,
                        PresenterBindingMode.Default,
                        new[] { view });

                    bindings.Add(binding);
                    boundViews.Add(view);
                }
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }
    }
}
