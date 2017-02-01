// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="Func{T, Boolean}"/> and <see cref="Predicate{T}"/>.
    /// </summary>
    public static class PredicateExtensions
    {
        public static Predicate<TSource> Negate<TSource>(this Predicate<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Predicate<TSource>>();

            return _ => !@this.Invoke(_);
        }

        public static Func<TSource, bool> Negate<TSource>(this Func<TSource, bool> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<Func<TSource, bool>>();

            return _ => !@this.Invoke(_);
        }
    }
}
