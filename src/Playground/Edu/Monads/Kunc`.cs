// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads
{
    public delegate Monad<T> Kunc<T>();

    public delegate Monad<TResult> Kunc<in T, TResult>(T arg);

    public delegate Monad<TResult> Kunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);
}
