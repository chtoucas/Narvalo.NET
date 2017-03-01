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
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Result<ExceptionDispatchInfo> Void => Result<ExceptionDispatchInfo>.Void;

        /// <summary>
        /// Obtains an instance of the <see cref="Result{TError}"/> class for the specified value.
        /// </summary>
        /// <typeparam name="TError">The underlying type of <paramref name="value"/>.</typeparam>
        /// <param name="value">A value to be wrapped into an object of type <see cref="Result{TError}"/>.</param>
        /// <returns>An instance of the <see cref="Result{TError}"/> class for the specified value.</returns>
        public static Result<TError> FromError<TError>(TError value) => Result<TError>.η(value);

        public static Result<T, TError> FromError<T, TError>(TError value) => Result<T, TError>.FromError(value);
    }

    public static partial class Result
    {
        public static void ThrowIfError(this Result<ExceptionDispatchInfo> @this)
        {
            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        public static void ThrowIfError<TException>(this Result<TException> @this) where TException : Exception
        {
            if (@this.IsError)
            {
                throw @this.Error;
            }
        }

        public static void ThrowIfError<T>(this Result<T, ExceptionDispatchInfo> @this)
        {
            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            if (@this.IsError)
            {
                throw @this.Error;
            }
        }

        // NB: This method serves a different purpose than the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Result<ExceptionDispatchInfo> TryWith(Action action)
        {
            Require.NotNull(action, nameof(action));

            try
            {
                action();

                return Result.Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<ExceptionDispatchInfo>(edi);
            }
        }

        // NB: This method is **not** the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult, ExceptionDispatchInfo> TryWith<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            try
            {
                return Of<TResult, ExceptionDispatchInfo>(func());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult, ExceptionDispatchInfo>(edi);
            }
        }

        // NB: This method serves a different purpose than the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of VoidOrError.")]
        public static Result<ExceptionDispatchInfo> TryFinally(Action action, Action finallyAction)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(finallyAction, nameof(finallyAction));

            try
            {
                action();

                return Result.Void;
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<ExceptionDispatchInfo>(edi);
            }
            finally
            {
                finallyAction();
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
                return Of<TResult, ExceptionDispatchInfo>(func());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult, ExceptionDispatchInfo>(edi);
            }
            finally
            {
                finallyAction();
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
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
