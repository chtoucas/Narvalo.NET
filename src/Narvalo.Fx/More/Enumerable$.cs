// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.More
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(funM, nameof(funM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return (from _ in @this
                    let m = funM.Invoke(_)
                    where m.IsSome
                    select m.Value).EmptyIfNull();
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> funM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(funM, nameof(funM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return (from _ in @this
                    let m = funM.Invoke(_)
                    where m.IsSuccess
                    select m.Value).EmptyIfNull();
        }
    }
}
