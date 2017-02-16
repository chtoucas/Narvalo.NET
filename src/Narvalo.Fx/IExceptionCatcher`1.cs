// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IExceptionCatcher<TException> where TException : Exception
    {
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        VoidOr<TException> Try(Action action);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<TResult>(Func<TResult> thunk);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<T, TResult>(Func<T, TResult> thunk, T arg);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<T1, T2, TResult>(Func<T1, T2, TResult> thunk, T1 arg1, T2 arg2);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> thunk, T1 arg1, T2 arg2, T3 arg3);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Try")]
        Result<TResult, TException> Try<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    }
}
