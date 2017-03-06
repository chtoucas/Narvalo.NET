// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    // Func<TState, StateObject<T, TState>>
    public delegate StateObject<T, TState> Computation<T, TState>(TState state);
}
