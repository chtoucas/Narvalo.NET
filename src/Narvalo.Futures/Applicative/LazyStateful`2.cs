// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    // Func<TState, Lazy<T, TState>>
    public delegate Lazy<T, TState> LazyStateful<T, TState>(TState state);

    // Provides the core Monad methods.
    public static partial class LazyStateful
    {
        public static LazyStateful<TNext, TState> Bind<T, TNext, TState>(
            this LazyStateful<T, TState> @this,
            Func<T, LazyStateful<TNext, TState>> selector)
            => state =>
            {
                Lazy<T, TState> obj = @this(state);

                return selector(obj.Value).Invoke(obj.Metadata);
            };

        // Initialize a stateless computation from a given value.
        public static LazyStateful<T, TState> Of<T, TState>(T value)
            => state => new Lazy<T, TState>(() => value, state);

        public static Stateful<T, TState> Flatten<T, TState>(Stateful<Stateful<T, TState>, TState> square)
            => square.Bind(Stubs<Stateful<T, TState>>.Identity);
    }

    public static partial class LazyStateful
    {
        public static LazyStateful<TState, TState> Get<TState>()
            => state => new Lazy<TState, TState>(() => state, state);

        public static LazyStateful<Unit, TState> Put<TState>(TState newState)
            => state => new Lazy<Unit, TState>(() => Unit.Default, newState);

        public static LazyStateful<Unit, TState> Modify<TState>(Func<TState, TState> func)
            => state => new Lazy<Unit, TState>(() => Unit.Default, func(state));

        public static LazyStateful<TResult, TState> Gets<TState, TResult>(Func<TState, TResult> func)
            => state => new Lazy<TResult, TState>(() => func(state), state);
    }
}
