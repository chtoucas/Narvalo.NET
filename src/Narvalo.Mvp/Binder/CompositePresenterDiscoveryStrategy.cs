// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.Internal;

    public sealed class CompositePresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        readonly IEnumerable<IPresenterDiscoveryStrategy> _strategies;
        readonly IEqualityComparer<IEnumerable<IView>> _comparer = new WeakEqualityComparer<IView>();

        public CompositePresenterDiscoveryStrategy(IEnumerable<IPresenterDiscoveryStrategy> strategies)
        {
            Require.NotNull(strategies, "strategies");

            // Force the strategies to be enumerated once, just in case somebody gave us an expensive
            // and uncached list.
            _strategies = strategies.ToArray();

            if (!strategies.Any()) {
                throw new ArgumentException("You must supply at least one strategy.", "strategies");
            }
        }

        public IEnumerable<PresenterDiscoveryResult> FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            var results = new List<PresenterDiscoveryResult>();

            var pendingViews = views;

            foreach (var strategy in _strategies) {
                if (!pendingViews.Any()) {
                    break;
                }

                var resultsThisRound = strategy.FindBindings(hosts, pendingViews);

                results.AddRange(resultsThisRound);

                var boundViews = from result in resultsThisRound
                                 from view in result.Views
                                 where result.Bindings.Any()
                                 select view;

                pendingViews = pendingViews.Except(boundViews.Distinct());
            }

            return results.GroupBy(_ => _.Views, _comparer).Select(CreateResult_);
        }

        static PresenterDiscoveryResult CreateResult_(
            IGrouping<IEnumerable<IView>, 
            PresenterDiscoveryResult> results)
        {
            return new PresenterDiscoveryResult(
                results.Key,
                results.SelectMany(_ => _.Bindings)
            );
        }
    }
}
