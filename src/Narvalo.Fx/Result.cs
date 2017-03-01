// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>
    /// and <see cref="Result{T}"/> and for querying objects that implement
    /// <see cref="IEnumerable{T}"/> where T is of type <see cref="Result{S, TError}"/> or
    /// <see cref="Result{S}"/>.
    /// </summary>
    public static partial class Result
    {
        public static Result<T> FromError<T>(ExceptionDispatchInfo exceptionInfo)
            => Result<T>.FromError(exceptionInfo);

        public static Result<T, TError> FromError<T, TError>(TError value)
            => Result<T, TError>.FromError(value);

        public static Result<T, TError> FlattenError<T, TError>(Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);
    }

    public static partial class Result
    {
        // NB: This method is **not** the trywith from F# workflows.
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

        // NB: This method is **not** the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static Result<TResult> TryFinally<TResult>(
            Func<TResult> func,
            Action finallyAction)
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

    // Provides extension methods for Result<T, TError>
    // where TError is of type Exception or ExceptionDispatchInfo.
    public static partial class Result
    {
        public static void ThrowIfError<T>(this Result<T, ExceptionDispatchInfo> @this)
        {
            if (@this.IsError) { @this.Error.Throw(); }
        }

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            if (@this.IsError) { throw @this.Error; }
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

        private static IEnumerable<TSource> CollectAnyIterator<TSource, TError>(
            IEnumerable<Result<TSource, TError>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }

    // Provides extension methods for IEnumerable<Result<T>>.
    public static partial class Result
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Result<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Result<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
