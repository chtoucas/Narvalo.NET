// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial struct Result
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Result s_Void = new Result();

        public static Result Void => s_Void;

        public static Result FromError(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));

            return new Result(exceptionInfo);
        }

        public static Result<T> FromError<T>(ExceptionDispatchInfo exceptionInfo)
            => Result<T>.FromError(exceptionInfo);

        public static Result<T, TError> FromError<T, TError>(TError error)
            => Result<T, TError>.FromError(error);

        public static Result<T, TError> FlattenError<T, TError>(Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);
    }

    public partial struct Result
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public static Result TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action.Invoke();
                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult> TryWith<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            try
            {
                return Of(func());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError<TResult>(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public static Result TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action.Invoke();
                return Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult> TryFinally<TResult>(Func<TResult> func, Action finallyAction)
        {
            Require.NotNull(func, nameof(func));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                return Of(func());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError<TResult>(edi);
            }
            finally
            {
                finallyAction();
            }
        }
    }
}
