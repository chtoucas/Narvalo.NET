// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>
    /// and <see cref="Result{T}"/> and for querying objects that implement
    /// <see cref="IEnumerable{T}"/> where T is of type <see cref="Result{S, TError}"/> or
    /// <see cref="Result{S}"/>.
    /// </summary>
    public static partial class ResultExtensions { }

    // Provides extension methods for Result<T, TError> where TError is of type Exception.
    public static partial class ResultExtensions
    {
        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            // NB: The Error property is never null.
            if (@this.IsError) { throw @this.Error; }
        }
    }

    // Provides extension methods for Result<bool, TError>.
    public static partial class ResultExtensions
    {
        public static bool IsTrue<TError>(this Result<bool, TError> @this)
            => !@this.IsError && @this.Value;

        public static bool IsFalse<TError>(this Result<bool, TError> @this)
            => @this.IsError || !@this.Value;
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class ResultExtensions
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
    public static partial class ResultExtensions
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
