// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Mvp.Properties;

    public sealed partial class CompositePresenterDiscoveryStrategy : IPresenterDiscoveryStrategy
    {
        private readonly IEnumerable<IPresenterDiscoveryStrategy> _strategies;

        public CompositePresenterDiscoveryStrategy(IEnumerable<IPresenterDiscoveryStrategy> strategies)
        {
            Require.NotNull(strategies, nameof(strategies));

            // Force the strategies to be enumerated once, just in case somebody gave us
            // an expensive and uncached list.
            _strategies = strategies.ToArray();

            if (!strategies.Any())
            {
                throw new ArgumentException(
                    Strings.CompositePresenterDiscoveryStrategy_StrategiesArrayIsEmpty,
                    nameof(strategies));
            }
        }

        public PresenterDiscoveryResult FindBindings(IEnumerable<object> hosts, IEnumerable<IView> views)
        {
            var bindings = new List<PresenterBindingParameter>();
            var boundViews = new List<IView>();

            var pendingViews = views;

            foreach (var strategy in _strategies)
            {
                if (strategy == null) { continue; }

                if (!pendingViews.Any())
                {
                    break;
                }

                var result = strategy.FindBindings(hosts, pendingViews);

                bindings.AddRange(result.Bindings);
                boundViews.AddRange(result.BoundViews);

                pendingViews = pendingViews.Except(result.BoundViews);
            }

            return new PresenterDiscoveryResult(boundViews, bindings);
        }
    }
}
