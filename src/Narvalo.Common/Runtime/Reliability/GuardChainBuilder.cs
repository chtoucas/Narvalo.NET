// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class GuardChainBuilder
    {
        private bool _closed = false;
        private IList<ISentinel> _guards = new List<ISentinel>();

        public GuardChainBuilder(ISentinel guard)
        {
            Require.NotNull(guard, "guard");

            Add(guard);
        }

        public IReadOnlyCollection<ISentinel> Guards
        {
            get { return new ReadOnlyCollection<ISentinel>(_guards); }
        }

        public void Add(ISentinel guard)
        {
            ThrowIfClosed();

            Require.NotNull(guard, "guard");

            ////if (!guard.IsChainable()) {
            ////    throw new Failure.Argument("guard");
            ////}

            _guards.Add(guard);
        }

        public void Add(IEnumerable<ISentinel> guards)
        {
            ThrowIfClosed();

            Require.NotNull(guards, "guards");

            foreach (var proxy in guards)
            {
                Add(proxy);
            }
        }

        public GuardChain Build()
        {
            ////IList<IGuard> guards = new List<IGuard>(_guards); 
            return new GuardChain(_guards);
        }

        public GuardChain Build(ISentinel guard)
        {
            Close(guard);
            return Build();
        }

        public void Close(ISentinel guard)
        {
            ThrowIfClosed();

            Require.NotNull(guard, "guard");

            _guards.Add(guard);
            _closed = true;
        }

        private void ThrowIfClosed()
        {
            if (_closed)
            {
                throw new InvalidOperationException("TODO");
            }
        }
    }
}
