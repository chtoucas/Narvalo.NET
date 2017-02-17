// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ITryCaptureExceptionInfo
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        VoidOrError Try(Action action);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        ResultOrError<TResult> Try<TResult>(Func<TResult> thunk);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        ResultOrError<TResult> Try<TSource, TResult>(Func<TSource, TResult> thunk, TSource value);
    }
}
