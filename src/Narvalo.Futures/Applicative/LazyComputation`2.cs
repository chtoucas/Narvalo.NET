// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;

    // Func<TState, Lazy<T, TState>>
    public delegate Lazy<T, TState> LazyComputation<T, TState>(TState state);
}
