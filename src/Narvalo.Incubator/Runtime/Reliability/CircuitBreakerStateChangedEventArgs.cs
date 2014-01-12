namespace Narvalo.Runtime.Reliability
{
    using System;

    public class CircuitBreakerStateChangedEventArgs : EventArgs
    {
        private readonly CircuitBreakerState _lastState;
        private readonly CircuitBreakerState _newState;

        public CircuitBreakerStateChangedEventArgs(
            CircuitBreakerState lastState, 
            CircuitBreakerState newState)
        {
            _lastState = lastState;
            _newState = newState;
        }

        public CircuitBreakerState LastState
        {
            get { return _lastState; }
        }

        public CircuitBreakerState NewState
        {
            get { return _newState; }
        }
    }
}
