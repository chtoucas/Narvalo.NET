// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;

    // TODO: Thread-Safety
    public sealed class CircuitBreaker : IReliabilitySentinel, IDisposable
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

        public double CurrentServiceLevel => 100D * (Threshold - _failureCount) / Threshold;

        public CircuitBreakerState CurrentState { get; private set; } = CircuitBreakerState.Closed;

        public int FailureCount { get { return _failureCount; } }

        public bool IsClosed => CurrentState == CircuitBreakerState.Closed;

        public bool IsHalfOpen => CurrentState == CircuitBreakerState.HalfOpen;

        public bool IsOpen => CurrentState == CircuitBreakerState.Open;

        public TimeSpan ResetInterval { get; }

        public int Threshold { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_resetTimer != null)
                {
                    _resetTimer.Dispose();
                    _resetTimer = null;
                }

                _disposed = true;
            }
        }

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            ThrowIfDisposed();

            Interlocked.MemoryBarrier();
            if (!CanInvoke)
            {
                // REVIEW: throw InvalidOperationException instead?
                throw new ReliabilityException("The circuit is open");
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
            Interlocked.Exchange(ref _failureCount, 0);
        }

        internal void Close() => Close(false /* executing */);

        internal void HalfOpen() => HalfOpen(false /* executing */);

        internal void Trip() => Trip(false /* executing */);

        #region State management.

        private void Close(bool executing)
        {
            Promise.Condition(!executing || IsHalfOpen);

            SetState(CircuitBreakerState.Closed);

            // FIXME: trop tard si SetState échoue.
            Interlocked.MemoryBarrier();
            if (executing && AutoReset)
            {
                StopTimer();
            }
        }

        private void HalfOpen(bool executing)
        {
            Promise.Condition(!executing || IsOpen);

            SetState(CircuitBreakerState.HalfOpen);

            // FIXME: trop tard si SetState échoue.
            Interlocked.MemoryBarrier();
            if (executing && AutoReset)
            {
                StopTimer();
            }
        }

        private void Trip(bool executing)
        {
            Promise.Condition(!executing || !IsOpen);

            SetState(CircuitBreakerState.Open);

            // FIXME: trop tard si SetState échoue.
            Interlocked.MemoryBarrier();
            if (executing && AutoReset)
            {
                StartTimer();
            }
        }

        private void OnStateChanged(CircuitBreakerStateChangedEventArgs e)
            => StateChangedEventHandler?.Invoke(this, e);

        #endregion

        #region Timer.

        // Create a timer but does not start it yet.
        // http://msdn.microsoft.com/en-us/magazine/cc164015.aspx
        private Timer CreateTimer()
        {
            Contract.Ensures(Contract.Result<Timer>() != null);

            // REVIEW: Put the reset logic into the callback?
            return new Timer(
                (state) =>
                {
                    Interlocked.MemoryBarrier();
                    if (IsOpen)
                    {
                        HalfOpen(true /* executing */);
                    }
                },
                null /* state */,
                Timeout.Infinite,
                Timeout.Infinite);
        }

        // NB: Timeout.Infinite = new TimeSpan(-1L)
        private void StartTimer() => _resetTimer.Change(ResetInterval, new TimeSpan(-1L));

        private void StopTimer() => _resetTimer.Change(Timeout.Infinite, Timeout.Infinite);

        #endregion

        private void RecordFailure()
        {
            Interlocked.MemoryBarrier();
            if (_failureCount < Threshold)
            {
                // Tant qu'on n'a pas atteint le seuil maximum d'erreurs, on incrémente le compteur.
                Interlocked.Increment(ref _failureCount);
            }

            bool openCircuit = IsHalfOpen || (IsClosed && _failureCount >= Threshold);

            if (openCircuit)
            {
                Trip(true /* executing */);
            }
        }

        private void RecordSuccess()
        {
            Interlocked.MemoryBarrier();
            if (_failureCount > 0)
            {
                Interlocked.Decrement(ref _failureCount);
            }

            Interlocked.MemoryBarrier();
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
