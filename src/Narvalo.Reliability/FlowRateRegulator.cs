namespace Narvalo.Reliability
{
    using System;
    using System.Threading;

    // FIXME: à refaire en checkpoint sans exception...
    public class FlowRateRegulator : IThrottle, IDisposable
    {
        private readonly TimeSpan _timeout;

        private bool _disposed = false;
        private bool _blocking = false;
        private Timer _resetTimer;

        public FlowRateRegulator(TimeSpan timeout)
        {
            _timeout = timeout;

            _resetTimer = new Timer((state) => { _blocking = false; }, null /* state */, new TimeSpan(0), timeout);
        }

        public bool Blocking { get { return _blocking; } }

        #region IStatefulGuard

        public bool CanExecute { get { return !_blocking; } }

        public int Multiplicity { get { return 1; } }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            if (CanExecute) {
                _blocking = true;
                action.Invoke();
            }
            else {
                throw new FlowRateExceededException();
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
                    if (_resetTimer != null) {
                        _resetTimer.Dispose();
                        _resetTimer = null;
                    }
                }

                _disposed = true;
            }
        }
    }
}
