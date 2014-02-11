// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    delegate Monad<T> Kunc<T>();

    delegate Monad<TResult> Kunc<in T, TResult>(T arg);
}
