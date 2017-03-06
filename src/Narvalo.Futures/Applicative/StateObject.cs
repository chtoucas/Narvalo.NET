// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    public static class StateObject
    {
        public static StateObject<T, TState> Create<T, TState>(T value, TState state)
            => new StateObject<T, TState>(value, state);
    }
}
