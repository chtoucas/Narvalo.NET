// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for querying objects
    /// that implement <see cref="IEnumerable{T}"/> where T is of type <see cref="Result{S, TError}"/>.
    /// </summary>
    public static partial class ResultSequence
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
