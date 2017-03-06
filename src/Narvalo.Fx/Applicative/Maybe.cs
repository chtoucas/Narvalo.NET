// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe { }

    // Provides extension methods for Maybe<T?> or Maybe<T> where T is a struct.
    // When it comes to value types, there is really no reason to use Maybe<T> or Maybe<T?>
    // instead of T?.
    public static partial class Maybe
    {
        // Conversion from T? to Maybe<T>.
        // NB: This method makes it impossible to create a Maybe<T?> **directly**.
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        // Conversion from Maybe<T> to T?.
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        // Conversion from Maybe<T?> to T?.
        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome ? @this.Value : null;
    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Maybe
    {
        // Named <c>catMaybes</c> in Haskell parlance.
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
