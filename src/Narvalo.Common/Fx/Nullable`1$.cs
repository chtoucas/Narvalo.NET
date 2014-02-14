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

        /*
         * What's not found here:
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

        public static TResult? Zip<T1, T2, T3, TResult>(
            this T1? @this,
            T2? second,
            T3? third,
            Func<T1, T2, T3, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where TResult : struct
        {
            return @this.HasValue && second.HasValue && third.HasValue
                ? (TResult?)resultSelector.Invoke(@this.Value, second.Value, third.Value)
                : null;
        }

        public static TResult? Zip<T1, T2, T3, T4, TResult>(
            this T1? @this,
            T2? second,
            T3? third,
            T4? fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where TResult : struct
        {
            return @this.HasValue && second.HasValue && third.HasValue && fourth.HasValue
                ? (TResult?)resultSelector.Invoke(@this.Value, second.Value, third.Value, fourth.Value)
                : null;
        }

        public static TResult? Zip<T1, T2, T3, T4, T5, TResult>(
            this T1? @this,
            T2? second,
            T3? third,
            T4? fourth,
            T5? fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            where T5 : struct
            where TResult : struct
        {
            return @this.HasValue && second.HasValue && third.HasValue && fourth.HasValue && fifth.HasValue
                ? (TResult?)resultSelector.Invoke(@this.Value, second.Value, third.Value, fourth.Value, fifth.Value)
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
