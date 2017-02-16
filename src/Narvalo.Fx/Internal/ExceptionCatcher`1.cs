// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;
    using System.Runtime.ExceptionServices;

    internal sealed class ExceptionCatcher<TException>
        : IExceptionCatcher
        where TException : Exception
    {
        public ExceptionCatcher() { }

        public VoidOrError TryInvoke(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.FromError(edi);
            }
        }

        public ResultOrError<TResult> TryInvoke<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                TResult result = thunk.Invoke();

                return ResultOrError.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return ResultOrError.FromError<TResult>(edi);
            }
        }

        public ResultOrError<TResult> TryInvoke<TSource, TResult>(
            Func<TSource, TResult> thunk,
            TSource value)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                TResult result = thunk.Invoke(value);

                return ResultOrError.Of(result);
            }
            catch (TException ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return ResultOrError.FromError<TResult>(edi);
            }
        }
    }
}
