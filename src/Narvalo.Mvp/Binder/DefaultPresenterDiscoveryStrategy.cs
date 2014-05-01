// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System.Collections.Generic;
    using System.Linq;
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

        readonly PresenterTypeProvider _presenterTypeProvider;

        public DefaultPresenterDiscoveryStrategy()
            : this(ViewInstanceSuffixes_, CandidatePresenterNames_) { }

        public DefaultPresenterDiscoveryStrategy(
            IEnumerable<string> viewInstanceSuffixes,
            IEnumerable<string> candidatePresenterNames)
        {
            _presenterTypeProvider = new CachedPresenterTypeProvider(
                new CachedViewInterfacesProvider(),
                viewInstanceSuffixes,
                candidatePresenterNames);
        }

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Require.NotNull(views, "views");

            // REVIEW: hosts is ignored.
            return views.Select(FindBinding_).ToArray();
        }

        PresenterDiscoveryResult FindBinding_(IView view)
        {
            var viewType = view.GetType();

            var presenterType = _presenterTypeProvider.GetComponent(viewType);

            return new PresenterDiscoveryResult(
                new[] { view },
                presenterType == null
                    ? new PresenterBinding[0]
                    : new[] { 
                        new PresenterBinding(
                            presenterType,
                            viewType, 
                            PresenterBindingMode.Default, 
                            new[] { view }) }
            );
        }
    }
}
