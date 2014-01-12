namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Narvalo.Diagnostics;

    public class GuardChainBuilder
    {
        private bool _closed = false;
        private IList<IGuard> _guards = new List<IGuard>();

        public GuardChainBuilder(IGuard guard)
        {
            Requires.NotNull(guard, "guard");

            Add(guard);
        }

        public ReadOnlyCollection<IGuard> Guards
        {
            get { return new ReadOnlyCollection<IGuard>(_guards); }
        }

        public void Add(IGuard guard)
        {
            ThrowIfClosed();

            Requires.NotNull(guard, "guard");

            //if (!guard.IsChainable()) {
            //    throw new ArgumentException("XXX", "guard");
            //}

            _guards.Add(guard);
        }

        public void Add(IEnumerable<IGuard> guards)
        {
            ThrowIfClosed();

            Requires.NotNull(guards, "guards");

            foreach (var proxy in guards) {
                Add(proxy);
            }
        }

        public GuardChain Build()
        {
            //IList<IGuard> guards = new List<IGuard>(_guards); 
            return new GuardChain(_guards);
        }

        public GuardChain Build(IGuard guard)
        {
            Close(guard);
            return Build();
        }

        public void Close(IGuard guard)
        {
            ThrowIfClosed();

            Requires.NotNull(guard, "guard");

            _guards.Add(guard);
            _closed = true;
        }

        private void ThrowIfClosed()
        {
            if (_closed) {
                throw new InvalidOperationException("XXX");
            }
        }
    }
}
