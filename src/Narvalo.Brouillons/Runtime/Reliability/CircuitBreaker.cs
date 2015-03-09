// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Threading;

    // IMPORTANT: Implémenter Thread-Safety
    public class CircuitBreaker : IBarrier, IDisposable
    {
        public event EventHandler<CircuitBreakerStateChangedEventArgs> StateChangedEventHandler;

        readonly TimeSpan _resetInterval;
        readonly int _threshold;

        bool _autoReset = false;
        bool _disposed = false;
        int _failureCount = 0;
        Timer _resetTimer;
        CircuitBreakerState _currentState = CircuitBreakerState.Closed;

        public CircuitBreaker(int threshold, TimeSpan resetInterval)
        {
            Require.GreaterThanOrEqualTo(threshold, 1, "threshold");

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

        public CircuitBreakerState CurrentState { get { return _currentState; } }

        public int FailureCount { get { return _failureCount; } }

        public bool IsClosed { get { return CurrentState == CircuitBreakerState.Closed; } }

        public bool IsHalfOpen { get { return CurrentState == CircuitBreakerState.HalfOpen; } }

        public bool IsOpen { get { return CurrentState == CircuitBreakerState.Open; } }

        public TimeSpan ResetInterval { get { return _resetInterval; } }

        public int Threshold { get { return _threshold; } }

        #endregion

        public void Reset()
        {
            StopTimer();
            SetState_(CircuitBreakerState.Closed);
            _failureCount = 0;
        }

        #region IBarrier

        public bool CanExecute { get { return !IsOpen; } }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            ThrowIfDisposed_();

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
                RecordFailure_();
                throw;
            }

            RecordSuccess_();
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

            SetState_(CircuitBreakerState.Closed);

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

            SetState_(CircuitBreakerState.HalfOpen);

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

            SetState_(CircuitBreakerState.Open);

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

        void RecordFailure_()
        {
            if (FailureCount < _threshold) {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                _failureCount++;
            }

            bool openCircuit = CurrentState == CircuitBreakerState.HalfOpen
                || (CurrentState == CircuitBreakerState.Closed && FailureCount >= _threshold);

            if (openCircuit) {
                Trip(true /* executing */);
            }
        }

        void RecordSuccess_()
        {
            if (FailureCount > 0) {
                _failureCount--;
            }

            if (IsHalfOpen) {
                Close(true /* executing */);
            }
        }

        void SetState_(CircuitBreakerState newState)
        {
            var lastState = CurrentState;
            _currentState = newState;
            OnStateChanged(new CircuitBreakerStateChangedEventArgs(lastState, newState));
        }

        void ThrowIfDisposed_()
        {
            if (_disposed) {
                throw new ObjectDisposedException(typeof(CircuitBreaker).FullName);
            }
        }

        #endregion
    }
}
