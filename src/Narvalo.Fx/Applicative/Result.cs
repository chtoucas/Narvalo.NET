// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/> where T is of type
    /// <see cref="Result{S, TError}"/>.
    /// </summary>
    public static partial class Result { }

    public static partial class Result
    {
        public static Result<T, TError> FlattenError<T, TError>(Result<T, Result<T, TError>> square)
            => Result<T, TError>.FlattenError(square);
    }

    // Provides extension methods for Result<T, TError> where TError is of type Exception.
    public static partial class Result
    {
        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            // NB: The Error property is never null.
            if (@this.IsError) { throw @this.Error; }
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class Result
    {
        public static IEnumerable<TSource> CollectAny<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> source)
        {
            Require.NotNull(source, nameof(source));

            return CollectAnyIterator(source);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource, TError>(
            IEnumerable<Result<TSource, TError>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
