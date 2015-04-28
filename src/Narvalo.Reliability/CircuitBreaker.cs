// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;

    // IMPORTANT: Implémenter Thread-Safety
    public class CircuitBreaker : IBarrier, IDisposable
    {
        private readonly TimeSpan _resetInterval;
        private readonly int _threshold;

        private bool _autoReset = false;
        private bool _disposed = false;
        private int _failureCount = 0;
        private Timer _resetTimer;
        private CircuitBreakerState _currentState = CircuitBreakerState.Closed;

        public CircuitBreaker(int threshold, TimeSpan resetInterval)
        {
            Require.GreaterThanOrEqualTo(threshold, 1, "threshold");

            _threshold = threshold;
            _resetInterval = resetInterval;
            _resetTimer = InitializeTimer();
        }

        public event EventHandler<CircuitBreakerStateChangedEventArgs> StateChangedEventHandler;

        public bool AutoReset
        {
            get
            {
                return _autoReset;
            }

            set
            {
                _autoReset = value;
                ////if (_autoReset & IsOpen) {
                ////    StartResetTimer();
                ////}
            }
        }

        public bool CanExecute { get { return !IsOpen; } }

        public double CurrentServiceLevel
        {
            get { return 100D * (Threshold - FailureCount) / Threshold; }
        }

        public CircuitBreakerState CurrentState { get { return _currentState; } }

        public int FailureCount { get { return _failureCount; } }

        public bool IsClosed { get { return CurrentState == CircuitBreakerState.Closed; } }

        public bool IsHalfOpen { get { return CurrentState == CircuitBreakerState.HalfOpen; } }

        public bool IsOpen { get { return CurrentState == CircuitBreakerState.Open; } }

        public TimeSpan ResetInterval { get { return _resetInterval; } }

        public int Threshold { get { return _threshold; } }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            ThrowIfDisposed();

            if (!CanExecute)
            {
                throw new InvalidOperationException("The circuit is open");
            }

            try
            {
                action.Invoke();
            }
            catch (GuardException)
            {
                throw;
            }
            catch
            {
                RecordFailure();
                throw;
            }

            RecordSuccess();
        }

        public void Reset()
        {
            ThrowIfDisposed();

            StopTimer();
            SetState(CircuitBreakerState.Closed);
            _failureCount = 0;
        }

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

        #region Gestion de l'état.

        protected virtual void Close(bool executing)
        {
            if (executing && CurrentState != CircuitBreakerState.HalfOpen)
            {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.Closed);

            // FIXME: trop tard si _setState échoue.
            if (AutoReset)
            {
                StopTimer();
            }
        }

        protected virtual void HalfOpen(bool executing)
        {
            if (executing && CurrentState != CircuitBreakerState.Open)
            {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.HalfOpen);

            // FIXME: trop tard si _setState échoue.
            if (AutoReset)
            {
                StopTimer();
            }
        }

        protected virtual void Trip(bool executing)
        {
            if (executing && CurrentState == CircuitBreakerState.Open)
            {
                throw new InvalidOperationException();
            }

            SetState(CircuitBreakerState.Open);

            // FIXME: trop tard si _setState échoue.
            if (executing && AutoReset)
            {
                StartTimer();
            }
        }

        protected virtual void OnStateChanged(CircuitBreakerStateChangedEventArgs e)
        {
            EventHandler<CircuitBreakerStateChangedEventArgs> localHandler = StateChangedEventHandler;

            if (localHandler != null)
            {
                localHandler(this, e);
            }
        }

        #endregion

        #region Timer.

        // http://msdn.microsoft.com/en-us/magazine/cc164015.aspx
        protected Timer InitializeTimer()
        {
            Contract.Ensures(Contract.Result<Timer>() != null);

            // On crée un timer mais on ne le démarre pas encore.
            // FIXME: ré-entrance
            var resetTimer = new Timer(
                (state) =>
                {
                    if (IsOpen)
                    {
                        HalfOpen(true /* executing */);
                    }
                },
                null,
                Timeout.Infinite,
                Timeout.Infinite);

            ////using Timer = System.Timers.Timer;
            ////_timer = new Timer(ResetInterval.TotalMilliseconds);
            ////_timer.Elapsed += (object sender, ElapsedEventArgs e) => {
            ////    if (IsOpen) HalfOpen(true /* executing */);
            ////};

            return resetTimer;
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

        private void RecordFailure()
        {
            if (FailureCount < _threshold)
            {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                _failureCount++;
            }

            bool openCircuit = CurrentState == CircuitBreakerState.HalfOpen
                || (CurrentState == CircuitBreakerState.Closed && FailureCount >= _threshold);

            if (openCircuit)
            {
                Trip(true /* executing */);
            }
        }

        private void RecordSuccess()
        {
            if (FailureCount > 0)
            {
                _failureCount--;
            }

            if (IsHalfOpen)
            {
                Close(true /* executing */);
            }
        }

        private void SetState(CircuitBreakerState newState)
        {
            var lastState = CurrentState;
            _currentState = newState;
            OnStateChanged(new CircuitBreakerStateChangedEventArgs(lastState, newState));
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(CircuitBreaker).FullName);
            }
        }
    }
}
