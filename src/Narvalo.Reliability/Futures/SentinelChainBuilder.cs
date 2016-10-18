// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class SentinelChainBuilder
    {
        private bool _closed = false;
        private IList<IReliabilitySentinel> _sentinels = new List<IReliabilitySentinel>();

        public SentinelChainBuilder(IReliabilitySentinel sentinel)
        {
            Require.NotNull(sentinel, nameof(sentinel));

            Add(sentinel);
        }

        public IReadOnlyCollection<IReliabilitySentinel> Sentinels
            => new ReadOnlyCollection<IReliabilitySentinel>(_sentinels);

        public void Add(IReliabilitySentinel sentinel)
        {
            Require.NotNull(sentinel, nameof(sentinel));

            ThrowIfClosed();

            ////if (!sentinel.IsChainable()) {
            ////    throw new Failure.Argument(nameof(sentinel));
            ////}

            _sentinels.Add(sentinel);
        }

        public void Add(IEnumerable<IReliabilitySentinel> sentinels)
        {
            Require.NotNull(sentinels, nameof(sentinels));

            ThrowIfClosed();

            foreach (var proxy in sentinels)
            {
                Add(proxy);
            }
        }

        public SentinelChain Build()
        {
            ////IList<ISentinel> sentinels = new List<ISentinel>(_sentinels);
            return new SentinelChain(_sentinels);
        }

        public SentinelChain Build(IReliabilitySentinel sentinel)
        {
            Close(sentinel);
            return Build();
        }

        public void Close(IReliabilitySentinel sentinel)
        {
            Require.NotNull(sentinel, nameof(sentinel));

            ThrowIfClosed();

            _sentinels.Add(sentinel);
            _closed = true;
        }

        private void ThrowIfClosed()
        {
            if (_closed)
            {
                throw new InvalidOperationException("XXX");
            }
        }
    }
}
