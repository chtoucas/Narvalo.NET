// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides extension methods for <see cref="System.Nullable{T}"/>.
    /// </summary>
    public static partial class NullableExtensions
    {
        #region Monad

        /*!
         * What's not to be found here:
         * - Return is simply casting: (T?)value
         * - Nullable does not support the Join operation; there is no Nullable<Nullable<T>>
         * - Zero is null
         */

        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? selector.Invoke(@this.Value) : null;
        }

        public static TResult? Map<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? (TResult?)selector.Invoke(@this.Value) : null;
        }

        public static TResult? Then<TSource, TResult>(this TSource? @this, TResult? other)
            where TSource : struct
            where TResult : struct
        {
            return @this.HasValue ? other : null;
        }

        #endregion

        #region Monadic lifting operators

        public static TResult? Zip<TFirst, TSecond, TResult>(
            this TFirst? @this,
            TSecond? second,
            Func<TFirst, TSecond, TResult> resultSelector)
            where TFirst : struct
            where TSecond : struct
            where TResult : struct
        {
            return @this.HasValue && second.HasValue
                ? (TResult?)resultSelector.Invoke(@this.Value, second.Value)
                : null;
        }

        #endregion

        #region Additional methods

        public static TResult? Coalesce<TSource, TResult>(
            this TSource? @this,
            Func<TSource, bool> predicate,
            TResult? then,
            TResult? otherwise)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }

        public static TResult? Then<TSource, TResult>(
            this TSource? @this,
            Func<TSource, bool> predicate,
            TResult? other)
            where TSource : struct
            where TResult : struct
        {
            return Coalesce(@this, predicate, other, null);
        }

        public static TResult? Otherwise<TSource, TResult>(
            this TSource? @this,
            Func<TSource, bool> predicate,
            TResult? other)
            where TSource : struct
            where TResult : struct
        {
            return Coalesce(@this, predicate, null, other);
        }

        public static Unit? Run<TSource>(this TSource? @this, Action<TSource> action)
            where TSource : struct
        {
            OnValue(@this, action);

            return (Unit?)Unit.Single;
        }

        #endregion

        //// ValueOrThrow

        public static TSource ValueOrThrow<TSource>(this TSource? @this, Exception exception)
            where TSource : struct
        {
            Require.NotNull(exception, "exception");

            return @this.ValueOrThrow(() => exception);
        }

        public static TSource ValueOrThrow<TSource>(this TSource? @this, Func<Exception> exceptionFactory)
            where TSource : struct
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (!@this.HasValue) {
                throw exceptionFactory.Invoke();
            }

            return @this.Value;
        }

        //// ToMaybe

        public static Maybe<TSource> ToMaybe<TSource>(this TSource? @this) where TSource : struct
        {
            return Maybe.Create(@this);
        }

        //// OnValue & OnNull

        public static TSource? OnValue<TSource>(this TSource? @this, Action<TSource> action)
            where TSource : struct
        {
            Require.NotNull(action, "action");

            if (@this.HasValue) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        public static TSource? OnNull<TSource>(this TSource? @this, Action action)
            where TSource : struct
        {
            Require.NotNull(action, "action");

            if (!@this.HasValue) {
                action.Invoke();
            }

            return @this;
        }
    }
}
