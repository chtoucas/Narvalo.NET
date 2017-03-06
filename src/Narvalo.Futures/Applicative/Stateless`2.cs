// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public sealed class Stateless<T, TState>
    {
        public Stateless(Computation<T, TState> computation)
        {
            Computation = computation;
        }

        internal Computation<T, TState> Computation { get; }

        public Stateless<TResult, TState> Bind<TResult>(Func<T, Stateless<TResult, TState>> selector)
        {
            Computation<TResult, TState> computation = state =>
            {
                StateObject<T, TState> obj = Computation(state);

                return selector(obj.Value).Computation(obj.State);
            };

            return new Stateless<TResult, TState>(computation);
        }

        // Initialize a stateless computation from a given value.
        public static Stateless<T, TState> Of(T value)
            => new Stateless<T, TState>(state => StateObject.Create(value, state));

        internal static Stateless<T, TState> μ(Stateless<Stateless<T, TState>, TState> square)
            => square.Bind(Stubs<Stateless<T, TState>>.Identity);
    }
}
