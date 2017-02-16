// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    internal sealed class ExceptionCatcher : IExceptionCatcher
    {
        public ExceptionCatcher() { }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public VoidOrError Try(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return VoidOrError.FromError(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public ResultOrError<TResult> Try<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                TResult result = thunk.Invoke();

                return ResultOrError.Of(result);
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return ResultOrError.FromError<TResult>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public ResultOrError<TResult> Try<TSource, TResult>(Func<TSource, TResult> thunk, TSource value)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                TResult result = thunk.Invoke(value);

                return ResultOrError.Of(result);
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return ResultOrError.FromError<TResult>(edi);
            }
        }
    }
}
