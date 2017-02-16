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
        public VoidOr<string> Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOr<string>>();

            try
            {
                action.Invoke();

                return VoidOr<string>.Void;
            }
            catch (Exception ex)
            {
                return VoidOr.FromError(ex.Message);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public VoidOrError Capture(Action action)
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
        public ResultOrError<TResult> Capture<TResult>(Func<TResult> thunk)
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
        public ResultOrError<TResult> Capture<TSource, TResult>(
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
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return ResultOrError.FromError<TResult>(edi);
            }
        }
    }
}
