// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe { }

    // Provides extension methods for Maybe<T?> or Maybe<T> where T is a struct.
    public static partial class Maybe
    {
        // Conversion from T? to Maybe<T>.
        // NB: This method makes it impossible to create a Maybe<T?> **directly**.
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        // FIXME: C# always ignores this method in favor of Maybe<T?>.Deconstruct().
        public static void Deconstruct<T>(
            this Maybe<T?> @this,
            out bool isSome,
            out T value)
            where T : struct
        {
            isSome = @this.IsSome;
            value = @this.IsSome ? @this.Value.Value : default(T);
        }

        // Conversion from Maybe<T?> to  Maybe<T>.
        public static Maybe<T> Flatten<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome
            ? Maybe.Of(@this.Value.Value)
            : Maybe<T>.None;

        // Conversion from Maybe<T> to T?.
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        // Conversion from Maybe<T?> to T?.
        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
#if DEBUG
            // In Debug mode, we protect the access to Value.
            => @this.IsSome ? @this.Value : null;
#else
            // If the object is "none", Value is default(T?) == null.
            => @this.Value;
#endif
    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Maybe
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return CollectAnyIterator(@this);
        }

        private static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
