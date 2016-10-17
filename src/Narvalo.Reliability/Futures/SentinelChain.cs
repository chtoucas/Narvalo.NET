// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public sealed class SentinelChain : IReliabilitySentinel
    {
        ////readonly Lazy<int> _multiplicityThunk;
        private readonly IEnumerable<IReliabilitySentinel> _sentinels;
        private readonly Lazy<IReadOnlyCollection<IReliabilitySentinel>> _sentinelsThunk;

        public SentinelChain(IEnumerable<IReliabilitySentinel> sentinels)
        {
            Require.NotNull(sentinels, nameof(sentinels));

            _sentinels = sentinels;
            _sentinelsThunk = new Lazy<IReadOnlyCollection<IReliabilitySentinel>>(
                () => new ReadOnlyCollection<IReliabilitySentinel>(_sentinels.ToList()));

            ////_multiplicityThunk
            ////    = new Lazy<int>(() => _sentinels.Aggregate(1 /* seed */, (a, g) => { return a * g.Multiplicity; }));
        }

        public IReadOnlyCollection<IReliabilitySentinel> Sentinels => _sentinelsThunk.Value;

        ////public int Multiplicity { get { return _multiplicityThunk.Value; } }

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            Action guardedAction = action;
            foreach (var sentinel in _sentinels)
            {
                guardedAction = () => sentinel.Invoke(guardedAction);
            }

            guardedAction.Invoke();
        }
    }
}
