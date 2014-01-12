namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Narvalo.Diagnostics;

    public class GuardChain : IGuard
    {
        //private readonly Lazy<int> _multiplicityThunk;
        private readonly IEnumerable<IGuard> _guards;
        private readonly Lazy<ReadOnlyCollection<IGuard>> _guardsThunk;

        public GuardChain(IEnumerable<IGuard> guards)
        {
            Requires.NotNull(guards, "guards");

            _guards = guards;
            _guardsThunk = new Lazy<ReadOnlyCollection<IGuard>>(() =>
                new ReadOnlyCollection<IGuard>(_guards.ToList())
            );

            //_multiplicityThunk
            //    = new Lazy<int>(() => _guards.Aggregate(1 /* seed */, (a, g) => { return a * g.Multiplicity; }));
        }

        public ReadOnlyCollection<IGuard> Guards
        {
            get { return _guardsThunk.Value; }
        }

        #region IGuard

        //public int Multiplicity { get { return _multiplicityThunk.Value; } }

        public void Execute(Action action)
        {
            Requires.NotNull(action, "action");

            Action guardedAction = action;
            foreach (var guard in _guards) {
                guardedAction = () => guard.Execute(guardedAction);
            }

            guardedAction();
        }

        #endregion
    }
}
