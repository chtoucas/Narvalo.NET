// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Threading;

    public class ConcurrencyThrottle : IReliabilitySentinel, IDisposable
    {
        private bool _disposed = false;
        private SemaphoreSlim _sem;

        public ConcurrencyThrottle(int maxConcurrentThreads, int millisecondsTimeout)
            : this(maxConcurrentThreads, TimeSpan.FromMilliseconds(millisecondsTimeout)) { }

        // FIXME: vérifier timeout pour ne pas attendre indéfininent ou timeout < 0.
        public ConcurrencyThrottle(int maxConcurrentThreads, TimeSpan timeout)
        {
            Require.Range(maxConcurrentThreads >= 1, nameof(maxConcurrentThreads));

            MaxConcurrentThreads = maxConcurrentThreads;
            Timeout = timeout;

            // NB: Contrairement à un Semaphore, SemaphoreSlim n'est pas partagé par l'ensemble
            // des processus de l'hôte.
            _sem = new SemaphoreSlim(maxConcurrentThreads);
        }

        //public bool CanInvoke => _sem.CurrentCount < MaxConcurrentThreads;

        public int MaxConcurrentThreads { get; }

        public TimeSpan Timeout { get; }

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            ThrowIfDisposed();

            if (_sem.Wait(Timeout))
            {
                try
                {
                    action.Invoke();
                }
                finally
                {
                    _sem.Release();
                }
            }
            else
            {
                throw new ReliabilityException("XXX");
            }
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
                    if (_sem != null)
                    {
                        _sem.Dispose();
                        _sem = null;
                    }
                }

                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(ConcurrencyThrottle).FullName);
            }
        }
    }
}
