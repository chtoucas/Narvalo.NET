// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    public sealed class Stateful<T, TState>
    {
        public Stateful(Computation<T, TState> computation)
        {
            Computation = computation;
        }

        internal Computation<T, TState> Computation { get; }

        public Stateful<TResult, TState> Bind<TResult>(Func<T, Stateful<TResult, TState>> selector)
        {
            Computation<TResult, TState> computation = state =>
            {
                StateObject<T, TState> obj = Computation(state);

                return selector(obj.Value).Computation(obj.State);
            };

            return new Stateful<TResult, TState>(computation);
        }

        // Create a stateful computation from a given value.
        public static Stateful<T, TState> Of(T value)
            => new Stateful<T, TState>(state => StateObject.Create(value, state));

        internal static Stateful<T, TState> μ(Stateful<Stateful<T, TState>, TState> square)
            => square.Bind(Stubs<Stateful<T, TState>>.Identity);
    }
}
