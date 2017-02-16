// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public interface IExceptionCatcher
    {
        VoidOrError TryInvoke(Action action);

        ResultOrError<TResult> TryInvoke<TResult>(Func<TResult> thunk);

        ResultOrError<TResult> TryInvoke<TSource, TResult>(Func<TSource, TResult> thunk, TSource value);
    }
}
