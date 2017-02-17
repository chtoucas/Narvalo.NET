// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Runtime.ExceptionServices;

    [Obsolete("BAD IDEA")]
    public sealed partial class TryCapture<T1Exception, T2Exception> : ITryCaptureExceptionInfo
        where T1Exception : Exception
        where T2Exception : Exception
    {
        internal TryCapture() { }

        public static VoidOrError With(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOrError>();

            ExceptionDispatchInfo edi;

            try
            {
                action.Invoke();

                return VoidOrError.Void;
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return VoidOrError.FromError(edi);
        }

        public static ResultOrError<TResult> With<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke();

                return ResultOrError.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return ResultOrError.FromError<TResult>(edi);
        }

        public static ResultOrError<TResult> With<TSource, TResult>(Func<TSource, TResult> thunk, TSource value)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<ResultOrError<TResult>>();

            ExceptionDispatchInfo edi;

            try
            {
                TResult result = thunk.Invoke(value);

                return ResultOrError.Of(result);
            }
            catch (T1Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }
            catch (T2Exception ex) { edi = ExceptionDispatchInfo.Capture(ex); }

            return ResultOrError.FromError<TResult>(edi);
        }
    }

    // Implements the ITryCapture interface.
    public sealed partial class TryCapture<T1Exception, T2Exception>
    {
        public VoidOrError Try(Action action) => With(action);

        public ResultOrError<TResult> Try<TResult>(Func<TResult> thunk) => With(thunk);

        public ResultOrError<TResult> Try<TSource, TResult>(Func<TSource, TResult> thunk, TSource value)
            => With(thunk, value);
    }
}
