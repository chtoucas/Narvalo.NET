// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public sealed partial class TryCatch<TException> : ITryCatch<TException>
        where TException : Exception
    {
        internal TryCatch() { }

        public static VoidOr<TException> With(Action action)
        {
            Require.NotNull(action, nameof(action));
            Warrant.NotNull<VoidOr<string>>();

            try
            {
                action.Invoke();

                return VoidOr<TException>.Void;
            }
            catch (TException ex)
            {
                return VoidOr.FromError(ex);
            }
        }

        public static Result<TResult, TException> With<TResult>(Func<TResult> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke();

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }

        public static Result<TResult, TException> With<T, TResult>(Func<T, TResult> thunk, T arg)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke(arg);

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }

        public static Result<TResult, TException> With<T1, T2, TResult>(
            Func<T1, T2, TResult> thunk, T1 arg1, T2 arg2)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke(arg1, arg2);

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }

        public static Result<TResult, TException> With<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> thunk, T1 arg1, T2 arg2, T3 arg3)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke(arg1, arg2, arg3);

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }

        public static Result<TResult, TException> With<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke(arg1, arg2, arg3, arg4);

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }

        public static Result<TResult, TException> With<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Require.NotNull(thunk, nameof(thunk));
            Warrant.NotNull<Result<TResult, TException>>();

            try
            {
                TResult result = thunk.Invoke(arg1, arg2, arg3, arg4, arg5);

                return Result.Of<TResult, TException>(result);
            }
            catch (TException ex)
            {
                return Result.FromError<TResult, TException>(ex);
            }
        }
    }

    // Implements the ITryCatch<TException> interface.
    public partial class TryCatch<TException>
    {
        public VoidOr<TException> Try(Action action)
            => With(action);

        public Result<TResult, TException> Try<TResult>(Func<TResult> thunk)
            => With(thunk);

        public Result<TResult, TException> Try<T, TResult>(Func<T, TResult> thunk, T arg)
            => With(thunk, arg);

        public Result<TResult, TException> Try<T1, T2, TResult>(Func<T1, T2, TResult> thunk, T1 arg1, T2 arg2)
            => With(thunk, arg1, arg2);

        public Result<TResult, TException> Try<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> thunk, T1 arg1, T2 arg2, T3 arg3)
            => With(thunk, arg1, arg2, arg3);

        public Result<TResult, TException> Try<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => With(thunk, arg1, arg2, arg3, arg4);

        public Result<TResult, TException> Try<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> thunk, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => With(thunk, arg1, arg2, arg3, arg4, arg5);
    }
}
