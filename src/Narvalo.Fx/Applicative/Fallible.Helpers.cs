// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    public partial struct Fallible
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public static Fallible TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action();
                return Ok;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Fallible<TResult> TryWith<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            try
            {
                return Of(func());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return Fallible<TResult>.FromError(edi);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public static Fallible TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action();
                return Ok;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return FromError(edi);
            }
            finally
            {
                finallyAction();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Fallible<TResult> TryFinally<TResult>(Func<TResult> func, Action finallyAction)
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
                return Fallible<TResult>.FromError(edi);
            }
            finally
            {
                finallyAction();
            }
        }
    }
}
