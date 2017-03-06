// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    // Func<TState, Lazy<T, TState>>
    public delegate Lazy<T, TState> LazyComputation<T, TState>(TState state);

    public static partial class LazyStateful
    {
        public static LazyComputation<TState, TState> Get<TState>()
            => state => new Lazy<TState, TState>(() => state, state);

        public static LazyComputation<Unit, TState> Put<TState>(TState state)
            => _ => new Lazy<Unit, TState>(() => Unit.Default, state);
    }
}
