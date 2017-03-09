// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    // Func<TState, (T, TState)>
    public delegate (T result, TState state) Stateful<T, TState>(TState state);

    // Provides the core Monad methods.
    public static partial class Stateful
    {
        public static Stateful<TNext, TState> Bind<T, TNext, TState>(
            this Stateful<T, TState> @this,
            Func<T, Stateful<TNext, TState>> binder)
            => state =>
            {
                var obj = @this(state);

                return binder(obj.result).Invoke(obj.state);
            };

        // Initialize a stateful computation from a given value.
        public static Stateful<T, TState> Of<T, TState>(T value)
            => state => (value, state);

        public static Stateful<T, TState> Flatten<T, TState>(Stateful<Stateful<T, TState>, TState> square)
            => square.Bind(Stubs<Stateful<T, TState>>.Identity);
    }

    // TODO: Find better names.
    public static partial class Stateful
    {
        public static Stateful<TState, TState> Get<TState>()
            => state => (state, state);

        public static Stateful<Unit, TState> Put<TState>(TState newState)
            => state => (Unit.Default, newState);

        public static Stateful<Unit, TState> Modify<TState>(Func<TState, TState> func)
            => state => (Unit.Default, func(state));

        public static Stateful<TResult, TState> Gets<TState, TResult>(Func<TState, TResult> func)
            => state => (func(state), state);
    }
}
