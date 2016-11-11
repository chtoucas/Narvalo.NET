// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Threading;

    public class FlowRateBarrier : IReliabilitySentinel, IDisposable
    {
        private bool _disposed = false;
        private int _requestCount = 0;
        private Timer _resetTimer;

        public FlowRateBarrier(int maxRequestsPerInterval, TimeSpan resetInterval)
        {
            Require.Range(maxRequestsPerInterval >= 1, nameof(maxRequestsPerInterval));

            MaxRequestsPerInterval = maxRequestsPerInterval;
            ResetInterval = resetInterval;

            ////_resetTimer = new Timer((state) => { _requestCount = 0; }, null /* state */, new TimeSpan(0), resetInterval);
            _resetTimer = new Timer(
                (state) =>
                {
                    _requestCount = 0;
                    _resetTimer.Change(ResetInterval, new TimeSpan(-1L));
                },
                null /* state */,
                ResetInterval,
                new TimeSpan(-1L));
        }

        public int MaxRequestsPerInterval { get; }

        public TimeSpan ResetInterval { get; }

        public bool CanInvoke => _requestCount <= MaxRequestsPerInterval;

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            ThrowIfDisposed();

            if (!CanInvoke)
            {
                throw new ReliabilityException("Flow rate exceeded.");
                //throw new InvalidOperationException("Flow rate exceeded.");
            }

            _requestCount++;
            action.Invoke();
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_resetTimer != null)
                    {
                        _resetTimer.Dispose();
                        _resetTimer = null;
                    }
                }

                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(FlowRateBarrier).FullName);
            }
        }
    }
}
