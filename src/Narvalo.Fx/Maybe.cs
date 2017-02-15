// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
        public static Maybe<T> Of<T>(T? value) where T : struct
            => value.HasValue ? Of(value.Value) : Maybe<T>.None;

        #region Extension methods when T is a struct

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
            => @this.IsSome ? (T?)@this.Value : null;

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
            => @this.ValueOrDefault();

        public static T ExtractOrDefault<T>(this Maybe<T?> @this) where T : struct
            => ExtractOrElse(@this, default(T));

        public static T ExtractOrElse<T>(this Maybe<T?> @this, T defaultValue) where T : struct
            => ExtractOrElse(@this, () => defaultValue);

        public static T ExtractOrElse<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.NotNull(defaultValueFactory, nameof(defaultValueFactory));

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T ExtractOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
            => ExtractOrThrow(@this, () => exception);

        public static T ExtractOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            T? m = @this.ValueOrDefault();

            if (!m.HasValue)
            {
                throw exceptionFactory.Invoke();
            }

            return m.Value;
        }

        #endregion
    }

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Maybe
    {
        // Named <c>catMaybes</c> in Haskell parlance.
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }
}
