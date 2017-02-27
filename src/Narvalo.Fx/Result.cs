// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Result{S, TError}"/>.
    /// </summary>
    public static partial class Result
    {
        public static Result<T, TError> FromError<T, TError>(TError value) => Result<T, TError>.η(value);

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                throw @this.Error;
            }
        }

        public static void ThrowIfError<T>(this Result<T, ExceptionDispatchInfo> @this)
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        // NB: This method serves a different purpose than the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Result<Unit, ExceptionDispatchInfo> TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action.Invoke();

                return Of<Unit, ExceptionDispatchInfo>(Unit.Default);
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<Unit, ExceptionDispatchInfo>(edi);
            }
        }

        // NB: This method is **not** the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult, ExceptionDispatchInfo> TryWith<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            try
            {
                return Of<TResult, ExceptionDispatchInfo>(func.Invoke());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult, ExceptionDispatchInfo>(edi);
            }
        }

        // NB: This method serves a different purpose than the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Result<Unit, ExceptionDispatchInfo> TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action.Invoke();

                return Of<Unit, ExceptionDispatchInfo>(Unit.Default);
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<Unit, ExceptionDispatchInfo>(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }

        // NB: This method is **not** the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult, ExceptionDispatchInfo> TryFinally<TResult>(
            Func<TResult> func,
            Action finallyAction)
        {
            Require.NotNull(func, nameof(func));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                return Of<TResult, ExceptionDispatchInfo>(func.Invoke());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult, ExceptionDispatchInfo>(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class Result
    {
        public static IEnumerable<TSource> CollectAny<TSource, TError>(this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource, TError>(IEnumerable<Result<TSource, TError>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TSource); }

                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
