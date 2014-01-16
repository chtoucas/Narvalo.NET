namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Threading;

    public class ConcurrencyThrottle : IGuard, IDisposable
    {
         readonly int _maxConcurrentRequests;
         readonly TimeSpan _timeout;

         bool _disposed = false;
         SemaphoreSlim _sem;

        // FIXME: vérifier timeout pour ne pas attendre indéfininent ou timeout < 0.
        public ConcurrencyThrottle(int maxConcurrentRequests, TimeSpan timeout)
        {
            Requires.GreaterThanOrEqualTo(maxConcurrentRequests, 1, "maxConcurrentRequests");

            _maxConcurrentRequests = maxConcurrentRequests;
            _timeout = timeout;

            // NB: Contrairement à un Semaphore, SemaphoreSlim n'est pas partagé par l'ensemble 
            // des processus de l'hôte.
            _sem = new SemaphoreSlim(maxConcurrentRequests);
        }

        public int MaxConcurrentRequests { get { return _maxConcurrentRequests; } }

        public TimeSpan Timeout { get { return _timeout; } }

        #region IGuard

        //public bool CanExecute
        //{
        //    get { return _sem.CurrentCount < _maxConcurrentRequests; }
        //}

        public void Execute(Action action)
        {
            ThrowIfDisposed();

            if (_sem.Wait(_timeout)) {
                try {
                    action();
                }
                finally {
                    _sem.Release();
                }
            }
            else {
                throw new ThrottleException();
            }
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
            if (!_disposed) {
                if (disposing) {
                    if (_sem != null) {
                        _sem.Dispose();
                        _sem = null;
                    }
                }

                _disposed = true;
            }
        }

        void ThrowIfDisposed()
        {
            if (_disposed) {
                throw new ObjectDisposedException(typeof(ConcurrencyThrottle).FullName);
            }
        }
    }
}
