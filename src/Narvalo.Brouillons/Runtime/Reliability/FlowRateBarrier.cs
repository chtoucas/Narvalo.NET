// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Threading;

    public class FlowRateBarrier : IBarrier, IDisposable
    {
        private readonly TimeSpan _resetInterval;
        private readonly int _maxRequestsPerInterval;

        private bool _disposed = false;
        private int _requestCount = 0;
        private Timer _resetTimer;

        public FlowRateBarrier(int maxRequestsPerInterval, TimeSpan resetInterval)
        {
            Require.GreaterThanOrEqualTo(maxRequestsPerInterval, 1, "maxRequestsPerInterval");

            _maxRequestsPerInterval = maxRequestsPerInterval;
            _resetInterval = resetInterval;

            ////_resetTimer = new Timer((state) => { _requestCount = 0; }, null /* state */, new TimeSpan(0), resetInterval);
            _resetTimer = new Timer(
                (state) =>
                {
                    _requestCount = 0;
                    _resetTimer.Change(_resetInterval, new TimeSpan(-1));
                },
                null /* state */,
                _resetInterval,
                new TimeSpan(-1));
        }

        public int MaxRequestsPerInterval { get { return _maxRequestsPerInterval; } }

        public TimeSpan ResetInterval { get { return _resetInterval; } }

        #region IBarrier

        public bool CanExecute
        {
            get { return _requestCount <= _maxRequestsPerInterval; }
        }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            ThrowIfDisposed();

            if (!CanExecute)
            {
                throw new FlowRateExceededException();
            }

            _requestCount++;
            action();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        #endregion

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
