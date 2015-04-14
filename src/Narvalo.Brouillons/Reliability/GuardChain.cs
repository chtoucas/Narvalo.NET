// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class GuardChain : ISentinel
    {
        ////readonly Lazy<int> _multiplicityThunk;
        private readonly IEnumerable<ISentinel> _guards;
        private readonly Lazy<IReadOnlyCollection<ISentinel>> _guardsThunk;

        public GuardChain(IEnumerable<ISentinel> guards)
        {
            Require.NotNull(guards, "guards");

            _guards = guards;
            _guardsThunk = new Lazy<IReadOnlyCollection<ISentinel>>(
                () => new ReadOnlyCollection<ISentinel>(_guards.ToList()));

            ////_multiplicityThunk
            ////    = new Lazy<int>(() => _guards.Aggregate(1 /* seed */, (a, g) => { return a * g.Multiplicity; }));
        }

        public IReadOnlyCollection<ISentinel> Guards
        {
            get { return _guardsThunk.Value; }
        }

        ////public int Multiplicity { get { return _multiplicityThunk.Value; } }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            Action guardedAction = action;
            foreach (var guard in _guards)
            {
                guardedAction = () => guard.Execute(guardedAction);
            }

            guardedAction();
        }
    }
}
