// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public interface IExceptionCatcher
    {
        VoidOrError Capture(Action action);

        ResultOrError<TResult> Capture<TResult>(Func<TResult> thunk);

        ResultOrError<TResult> Capture<TSource, TResult>(Func<TSource, TResult> thunk, TSource value);
    }
}
