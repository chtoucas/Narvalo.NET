// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    public static partial class Stateful
    {
        public static Stateful<TState, TState> Get<TState>()
            => new Stateful<TState, TState>(state => StateObject.Create(state, state));

        public static Stateful<Unit, TState> Put<TState>(TState newState)
            => new Stateful<Unit, TState>(_ => StateObject.Create(Unit.Default, newState));
    }
}
