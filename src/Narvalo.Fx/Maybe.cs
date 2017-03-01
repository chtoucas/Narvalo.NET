﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="Maybe{T}"/>
    /// and for querying objects that implement <see cref="IEnumerable{T}"/>
    /// where T is of type <see cref="Maybe{S}"/>.
    /// </summary>
    public static partial class Maybe
    {
        // Conversion from T? to Maybe<T>.
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        // Conversion from Maybe<T> to T?.
        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;
    }

    // Provides extension methods for Maybe<T?>.
    // NB: in fatc, there is really no reason to use Maybe<T> or Maybe<T?> instead of T? (for value
    // types of course).
    public static partial class Maybe
    {
        public static Maybe<T> Flatten<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome ? Maybe.Of(@this.Value) : Maybe<T>.None;

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.IsSome ? @this.Value : null;

        // REVIEW: Returning @this.Value should be enough.
        public static T Unwrap<T>(this Maybe<T?> @this) where T : struct
            => @this.ValueOrDefault() ?? default(T);

        public static T Unwrap<T>(this Maybe<T?> @this, T defaultValue) where T : struct
            => @this.ValueOrDefault() ?? defaultValue;

        public static T Unwrap<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, nameof(defaultValueFactory));

            return @this.ValueOrDefault() ?? defaultValueFactory();
        }

        public static T UnwrapOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
            => UnwrapOrThrow(@this, () => exception);

        public static T UnwrapOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory();
            }

            return m.Value;
        }
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

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
