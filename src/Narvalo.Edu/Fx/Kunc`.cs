// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System.Diagnostics.CodeAnalysis;

    public delegate Monad<T> Kunc<T>();

    public delegate Monad<TResult> Kunc<in T, TResult>(T arg);

    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes")]
    public delegate Monad<TResult> Kunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);
}
