// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="ResultOrError{T}"/>.
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="ResultOrError{S}"/>.
    /// </summary>
    public static partial class ResultOrError
    {
        public static ResultOrError<T> FromError<T>(ExceptionDispatchInfo exceptionInfo)
        {
            Expect.NotNull(exceptionInfo);
            Warrant.NotNull<ResultOrError<T>>();

            return ResultOrError<T>.η(exceptionInfo);
        }

        // NB: This method is **not** the trywith from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static ResultOrError<TResult> TryWith<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                return Of(func.Invoke());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult>(edi);
            }
        }

        // NB: This method is **not** the tryfinally from F# workflows.
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of ResultOrError.")]
        public static ResultOrError<TResult> TryFinally<TResult>(Func<TResult> func, Action finallyAction)
        {
            Require.NotNull(func, nameof(func));
            Require.NotNull(finallyAction, nameof(finallyAction));
            Warrant.NotNull<ResultOrError<TResult>>();

            try
            {
                return Of(func.Invoke());
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);

                return FromError<TResult>(edi);
            }
            finally
            {
                finallyAction.Invoke();
            }
        }
    }

    // Provides extension methods for IEnumerable<ResultOrError<T>>.
    public static partial class ResultOrError
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<ResultOrError<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<ResultOrError<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TSource); }

                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
