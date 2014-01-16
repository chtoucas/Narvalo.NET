namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Threading;

    public class CircuitBreaker : IBarrier, IDisposable
    {
        public event EventHandler<CircuitBreakerStateChangedEventArgs> StateChangedEventHandler;

        readonly TimeSpan _resetInterval;
        readonly int _threshold;

        bool _autoReset = false;
        bool _disposed = false;
        int _failureCount = 0;
        Timer _resetTimer;
        CircuitBreakerState _state = CircuitBreakerState.Closed;

        public CircuitBreaker(int threshold, TimeSpan resetInterval)
        {
            Requires.GreaterThanOrEqualTo(threshold, 1, "threshold");

            _threshold = threshold;
            _resetInterval = resetInterval;

            InitializeTimer();
        }

        #region Propriétés

        public bool AutoReset
        {
            get { return _autoReset; }
            set
            {
                _autoReset = value;
                //if (_autoReset & IsOpen) {
                //    StartResetTimer();
                //}
            }
        }

        public double CurrentServiceLevel
        {
            get { return 100 * (Threshold - FailureCount) / Threshold; }
        }

        public CircuitBreakerState CurrentState { get { return _state; } private set { _state = value; } }

        public int FailureCount { get { return _failureCount; } private set { _failureCount = value; } }

        public bool IsClosed { get { return CurrentState == CircuitBreakerState.Closed; } }

        public bool IsHalfOpen { get { return CurrentState == CircuitBreakerState.HalfOpen; } }

        public bool IsOpen { get { return CurrentState == CircuitBreakerState.Open; } }

        public TimeSpan ResetInterval { get { return _resetInterval; } }

        public int Threshold { get { return _threshold; } }

        #endregion

        public void Reset()
        {
            StopTimer();
            SetState(CircuitBreakerState.Closed);
            FailureCount = 0;
        }

        #region IBarrier

        public bool CanExecute { get { return !IsOpen; } }

        public void Execute(Action action)
        {
            Requires.NotNull(action, "action");

            ThrowIfDisposed();

            if (!CanExecute) {
                throw new CircuitOpenException();
            }

            try {
                action();
            }
            catch (GuardException) {
                throw;
            }
            catch {
                RecordFailure();
                throw;
            }

            RecordSuccess();
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

        #region Gestion de l'état.

        protected virtual void Close(bool executing)
        {
            if (executing && CurrentState != CircuitBreakerState.HalfOpen) {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.Closed);

            // FIXME: trop tard si _setState échoue.
            if (AutoReset) {
                StopTimer();
            }
        }

        protected virtual void HalfOpen(bool executing)
        {
            if (executing && CurrentState != CircuitBreakerState.Open) {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.HalfOpen);

            // FIXME: trop tard si _setState échoue.
            if (AutoReset) {
                StopTimer();
            }
        }

        protected virtual void Trip(bool executing)
        {
            if (executing && CurrentState == CircuitBreakerState.Open) {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.Open);

            // FIXME: trop tard si _setState échoue.
            if (executing && AutoReset) {
                StartTimer();
            }
        }

        protected virtual void OnStateChanged(CircuitBreakerStateChangedEventArgs e)
        {
            EventHandler<CircuitBreakerStateChangedEventArgs> localHandler = StateChangedEventHandler;

            if (localHandler != null) {
                localHandler(this, e);
            }
        }

        #endregion

        #region Timer.

        // http://msdn.microsoft.com/en-us/magazine/cc164015.aspx
        protected void InitializeTimer()
        {
            // On crée un timer mais on ne le démarre pas encore.
            // FIXME: ré-entrance
            _resetTimer = new Timer(
                (state) =>
                {
                    if (IsOpen) {
                        HalfOpen(true /* executing */);
                    }
                },
                null, Timeout.Infinite, Timeout.Infinite);

            //using Timer = System.Timers.Timer;
            //_timer = new Timer(ResetInterval.TotalMilliseconds);
            //_timer.Elapsed += (object sender, ElapsedEventArgs e) => {
            //    if (IsOpen) HalfOpen(true /* executing */);
            //};
        }

        protected void StartTimer()
        {
            _resetTimer.Change(ResetInterval, new TimeSpan(-1));
        }

        protected void StopTimer()
        {
            _resetTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Membres internes.

        internal void Close()
        {
            Close(false /* executing */);
        }

        internal void HalfOpen()
        {
            HalfOpen(false /* executing */);
        }

        internal void Trip()
        {
            Trip(false /* executing */);
        }

        #endregion

        #region Membres privés.

        private void RecordFailure()
        {
            if (FailureCount < _threshold) {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                FailureCount++;
            }

            bool openCircuit = CurrentState == CircuitBreakerState.HalfOpen
                || (CurrentState == CircuitBreakerState.Closed && FailureCount >= _threshold);

            if (openCircuit) {
                Trip(true /* executing */);
            }
        }

        private void RecordSuccess()
        {
            if (FailureCount > 0) {
                FailureCount++;
            }

            if (IsHalfOpen) {
                Close(true /* executing */);
            }
        }

        private void SetState(CircuitBreakerState newState)
        {
            var lastState = CurrentState;
            CurrentState = newState;
            OnStateChanged(new CircuitBreakerStateChangedEventArgs(lastState, newState));
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) {
                throw new ObjectDisposedException(typeof(CircuitBreaker).FullName);
            }
        }

        #endregion
    }
}
