// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public interface IExceptionCatcher
    {
        VoidOrError Invoke(Action action);

        Outcome<TResult> Invoke<TResult>(Func<TResult> thunk);

        Outcome<TResult> Invoke<TSource, TResult>(Func<TSource, TResult> thunk, TSource value);
    }
}
