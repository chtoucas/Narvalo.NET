// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public static partial class Stateless
    {
        public static Stateless<TState, TState> Get<TState>()
            => new Stateless<TState, TState>(state => StateObject.Create(state, state));

        public static Stateless<Unit, TState> Put<TState>(TState newState)
            => new Stateless<Unit, TState>(_ => StateObject.Create(Unit.Default, newState));

        public static Stateless<Unit, TState> Modify<TState>(Func<TState, TState> func)
            => new Stateless<Unit, TState>(state => StateObject.Create(Unit.Default, func(state)));

        public static Stateless<TResult, TState> Gets<TState, TResult>(Func<TState, TResult> func)
            => new Stateless<TResult, TState>(state => StateObject.Create(func(state), state));


        public static LazyComputation<TState, TState> GetLazy<TState>()
            => state => new Lazy<TState, TState>(() => state, state);

        public static LazyComputation<Unit, TState> PutLazy<TState>(TState newState)
            => state => new Lazy<Unit, TState>(() => Unit.Default, newState);

        public static LazyComputation<Unit, TState> ModifyLazy<TState>(Func<TState, TState> func)
            => state => new Lazy<Unit, TState>(() => Unit.Default, func(state));
    }
}
