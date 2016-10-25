// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;

    // TODO: Thread-Safety
    public class CircuitBreaker : IReliabilitySentinel, IDisposable
    {
        private bool _autoReset = false;
        private bool _disposed = false;
        private Timer _resetTimer;

        private int _failureCount = 0;

        public CircuitBreaker(int threshold, TimeSpan resetInterval)
        {
            Require.GreaterThanOrEqualTo(threshold, 1, nameof(threshold));

            Threshold = threshold;
            ResetInterval = resetInterval;
            _resetTimer = CreateTimer();
        }

        public event EventHandler<CircuitBreakerStateChangedEventArgs> StateChangedEventHandler;

        // REVIEW: read-only?
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

        // REVIEW: Spelling: Open or Opened?
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
                // REVIEW: throw new InvalidOperationException("The circuit is open");
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
            //FailureCount = 0;
            Interlocked.Exchange(ref _failureCount, 0);
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
            if (executing)
            {
                Check.Condition(IsHalfOpen, "XXX");
            }
            //if (executing && !IsHalfOpen)
            //{
            //    // REVIEW: ReliabilityException?
            //    throw new InvalidOperationException("XXX");
            //}

            SetState(CircuitBreakerState.Closed);

            // FIXME: trop tard si SetState échoue.
            if (executing && AutoReset)
            {
                StopTimer();
            }
        }

        protected virtual void HalfOpen(bool executing)
        {
            if (executing)
            {
                Check.Condition(IsOpen, "XXX");
            }
            //if (executing && !IsOpen)
            //{
            //    // REVIEW: ReliabilityException?
            //    throw new InvalidOperationException("XXX");
            //}

            SetState(CircuitBreakerState.HalfOpen);

            // FIXME: trop tard si SetState échoue.
            if (executing && AutoReset)
            {
                StopTimer();
            }
        }

        protected virtual void Trip(bool executing)
        {
            if (executing)
            {
                Check.Condition(!IsOpen, "XXX");
            }
            //if (executing && IsOpen)
            //{
            //    // REVIEW: ReliabilityException?
            //    throw new InvalidOperationException("XXX");
            //}

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
        protected Timer CreateTimer()
        {
            Contract.Ensures(Contract.Result<Timer>() != null);

            // Create a timer but does not start it yet.
            // FIXME: reentrancy?
            var resetTimer = new Timer(
                (state) =>
                {
                    if (IsOpen)
                    {
                        HalfOpen(true /* executing */);
                    }
                },
                null /* state */,
                Timeout.Infinite,
                Timeout.Infinite);

            return resetTimer;
        }

        // NB: Timeout.Infinite = new TimeSpan(-1L)
        protected void StartTimer() => _resetTimer.Change(ResetInterval, new TimeSpan(-1L));

        protected void StopTimer() => _resetTimer.Change(Timeout.Infinite, Timeout.Infinite);

        #endregion

        private void RecordFailure()
        {
            if (FailureCount < Threshold)
            {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                Interlocked.Increment(ref _failureCount);
                //FailureCount++;
            }

            bool openCircuit = IsHalfOpen || (IsClosed && FailureCount >= Threshold);

            if (openCircuit)
            {
                Trip(true /* executing */);
            }
        }

        private void RecordSuccess()
        {
            if (FailureCount > 0)
            {
                //FailureCount--;
                Interlocked.Decrement(ref _failureCount);
            }

            if (IsHalfOpen)
            {
                Close(true /* executing */);
            }
        }

        private void SetState(CircuitBreakerState newState)
        {
            CircuitBreakerState lastState = CurrentState;
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
