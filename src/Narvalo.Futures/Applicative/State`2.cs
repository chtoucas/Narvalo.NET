// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    // class Monad m => MonadState s m | m -> s where
    public delegate Lazy<T, TState> State<T, TState>(TState state);

    public static partial class Stateful
    {
        // Return the state from the internals of the monad.
        // get :: m s
        // get = state (\s -> (s, s))
        public static State<TState, TState> Get<TState>()
            => state => new Lazy<TState, TState>(() => state, state);

        // Replace the state inside the monad.
        // put :: s -> m ()
        // put s = state (\_ -> ((), s))
        public static State<Unit, TState> Put<TState>(TState state)
            => _ => new Lazy<Unit, TState>(() => Unit.Default, state);
    }
}
