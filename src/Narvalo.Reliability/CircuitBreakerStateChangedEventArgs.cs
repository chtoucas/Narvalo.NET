// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public class CircuitBreakerStateChangedEventArgs : EventArgs
    {
        public CircuitBreakerStateChangedEventArgs(
            CircuitBreakerState lastState,
            CircuitBreakerState newState)
        {
            LastState = lastState;
            NewState = newState;
        }

        public CircuitBreakerState LastState { get; }

        public CircuitBreakerState NewState { get; }
    }
}
