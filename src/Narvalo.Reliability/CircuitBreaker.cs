// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;

    // IMPORTANT: Implémenter Thread-Safety
    public class CircuitBreaker : IReliabilitySentinel, IDisposable
    {
        private bool _autoReset = false;
        private bool _disposed = false;
        private Timer _resetTimer;

        public CircuitBreaker(int threshold, TimeSpan resetInterval)
        {
            Require.GreaterThanOrEqualTo(threshold, 1, nameof(threshold));

            Threshold = threshold;
            ResetInterval = resetInterval;
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
                ////    StartTimer();
                ////}
            }
        }

        public bool CanInvoke => !IsOpen;

        public double CurrentServiceLevel => 100D * (Threshold - FailureCount) / Threshold;

        public CircuitBreakerState CurrentState { get; private set; } = CircuitBreakerState.Closed;

        public int FailureCount { get; private set; } = 0;

        public bool IsClosed => CurrentState == CircuitBreakerState.Closed;

        // REVIEW: Opened?
        public bool IsHalfOpen => CurrentState == CircuitBreakerState.HalfOpen;

        public bool IsOpen => CurrentState == CircuitBreakerState.Open;

        public TimeSpan ResetInterval { get; }

        public int Threshold { get; }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            ThrowIfDisposed();

            if (!CanInvoke)
            {
                throw new ReliabilityException("The circuit is open");
                //throw new InvalidOperationException("The circuit is open");
            }

            try
            {
                action.Invoke();
            }
            catch (ReliabilityException)
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
            FailureCount = 0;
        }

        internal void Close() => Close(false /* executing */);

        internal void HalfOpen() => HalfOpen(false /* executing */);

        internal void Trip() => Trip(false /* executing */);

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

            // FIXME: trop tard si SetState échoue.
            if (AutoReset)
            {
                StopTimer();
            }
        }

        protected virtual void HalfOpen(bool executing)
        {
            if (executing && CurrentState != CircuitBreakerState.Open)
            {
                throw new InvalidOperationException("XXX");
            }

            SetState(CircuitBreakerState.HalfOpen);

            // FIXME: trop tard si SetState échoue.
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

            // FIXME: trop tard si SetState échoue.
            if (executing && AutoReset)
            {
                StartTimer();
            }
        }

        protected virtual void OnStateChanged(CircuitBreakerStateChangedEventArgs e)
            => StateChangedEventHandler?.Invoke(this, e);

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

        protected void StartTimer() => _resetTimer.Change(ResetInterval, new TimeSpan(-1L));

        protected void StopTimer() => _resetTimer.Change(Timeout.Infinite, Timeout.Infinite);

        #endregion

        private void RecordFailure()
        {
            if (FailureCount < Threshold)
            {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                FailureCount++;
            }

            bool openCircuit = CurrentState == CircuitBreakerState.HalfOpen
                || (CurrentState == CircuitBreakerState.Closed && FailureCount >= Threshold);

            if (openCircuit)
            {
                Trip(true /* executing */);
            }
        }

        private void RecordSuccess()
        {
            if (FailureCount > 0)
            {
                FailureCount--;
            }

            if (IsHalfOpen)
            {
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
            if (_disposed)
            {
                throw new ObjectDisposedException(typeof(CircuitBreaker).FullName);
            }
        }
    }
}
