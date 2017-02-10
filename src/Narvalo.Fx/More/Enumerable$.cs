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
        // Named <c>mapMaybe</c> in Haskell parlance.
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> selectorM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selectorM, nameof(selectorM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return (from _ in @this
                    let m = selectorM.Invoke(_)
                    where m.IsSome
                    select m.Value).EmptyIfNull();
        }

        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Outcome<TResult>> selectorM)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selectorM, nameof(selectorM));
            Warrant.NotNull<IEnumerable<TResult>>();

            return (from _ in @this
                    let m = selectorM.Invoke(_)
                    where m.IsSuccess
                    select m.Value).EmptyIfNull();
        }
    }
}
