// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Either{TLeft, TRight}"/>.
    /// </summary>
    public static partial class Either
    {
        public static Either<TLeft, TRight> FlattenLeft<TLeft, TRight>(Either<Either<TLeft, TRight>, TRight> square)
            => Either<TLeft, TRight>.μ(square);

        public static Either<TLeft, TRight> FlattenRight<TLeft, TRight>(Either<TLeft, Either<TLeft, TRight>> square)
            => Either<TLeft, TRight>.FlattenRight(square);
    }

    // Provides extension methods for Either<TLeft, TRight>.
    public static partial class Either
    {
        public static Either<TResult, TRight> SelectLeft<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TLeft, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.BindLeft(val => Either<TResult, TRight>.OfLeft(selector(val)));
        }

        public static Either<TLeft, TResult> SelectRight<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TRight, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return @this.BindRight(val => Either<TLeft, TResult>.OfRight(selector(val)));
        }
    }

    // Provides extension methods for IEnumerable<Either<T, TError>>.
    public static partial class Either
    {
        public static IEnumerable<TLeft> CollectAny<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TLeft> CollectAnyIterator<TLeft, TRight>(
            IEnumerable<Either<TLeft, TRight>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsLeft) { yield return item.Left; }
            }
        }
    }
}
