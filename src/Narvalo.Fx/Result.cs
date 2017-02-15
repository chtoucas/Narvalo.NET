// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.ExceptionServices;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Result{T, TError}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Result{S, TError}"/>.
    /// </summary>
    public static partial class Result
    {
        public static Result<T, TError> FromError<T, TError>(TError value)
        {
            Warrant.NotNull<Result<T, TError>>();

            return Result<T, TError>.η(value);
        }

        public static void ThrowIfError<T>(this Result<T, ExceptionDispatchInfo> @this)
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                @this.Error.Throw();
            }
        }

        public static void ThrowIfError<T, TException>(this Result<T, TException> @this) where TException : Exception
        {
            Require.NotNull(@this, nameof(@this));

            if (@this.IsError)
            {
                throw @this.Error;
            }
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class Result
    {
        public static IEnumerable<TSource> CollectAny<TSource, TError>(this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource, TError>(IEnumerable<Result<TSource, TError>> source)
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
