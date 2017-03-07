// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using System;

    using Narvalo;
    using Narvalo.Applicative;

    /// <summary>
    /// The Nullable monad
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>What's not to be found here:</listheader>
    /// <item><description><c>Pure</c> is simply casting: <c>(T?)value</c>.</description></item>
    /// <item><description><c>Nullable</c> does not support the <c>Join</c> operation; there is no
    /// <c>Nullable&lt;Nullable&lt;T&gt;&gt;</c>.</description></item>
    /// <item><description><c>Zero</c> is null.</description></item>
    /// </list>
    /// </remarks>
    public static class Nullable
    {
        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, nameof(selector));

            return @this.HasValue ? selector(@this.Value) : null;
        }

        public static TResult? ContinueWith<TSource, TResult>(this TSource? @this, TResult? other)
            where TSource : struct
            where TResult : struct
            => @this.HasValue ? other : null;

        public static TResult? Zip<T1, T2, TResult>(
            this T1? @this,
            T2? second,
            Func<T1, T2, TResult> zipper)
            where T1 : struct
            where T2 : struct
            where TResult : struct
        {
            Require.NotNull(zipper, nameof(zipper));

            return @this.HasValue && second.HasValue
                ? (TResult?)zipper(@this.Value, second.Value)
                : null;
        }

        #region Query Expression Pattern

        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, nameof(predicate));

            return @this.HasValue && predicate(@this.Value) ? @this : null;
        }

        public static TResult? SelectMany<TSource, TMiddle, TResult>(
            this TSource? @this,
            Func<TSource, TMiddle?> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(valueSelector, nameof(valueSelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            return @this.Bind(_ => valueSelector(_).Select(middle => resultSelector(_, middle)));
        }

        #endregion

        #region Pattern matching

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> caseValue,
            Func<TResult> caseNull)
            where TSource : struct
        {
            Require.NotNull(caseValue, nameof(caseValue));
            Require.NotNull(caseNull, nameof(caseNull));

            return @this.HasValue ? caseValue(@this.Value) : caseNull();
        }

        public static TResult Match<TSource, TResult>(
            this TSource? @this,
            Func<TSource, TResult> caseValue,
            TResult caseNull)
            where TSource : struct
        {
            Require.NotNull(caseValue, nameof(caseValue));

            return @this.HasValue ? caseValue(@this.Value) : caseNull;
        }

        public static void Do<TSource>(this TSource? @this, Action<TSource> onValue, Action onNull)
            where TSource : struct
        {
            Require.NotNull(onValue, nameof(onValue));
            Require.NotNull(onNull, nameof(onNull));

            if (@this.HasValue)
            {
                onValue(@this.Value);
            }
            else
            {
                onNull();
            }
        }

        #endregion

        public static TSource ValueOrThrow<TSource>(this TSource? @this, Exception exception)
            where TSource : struct
        {
            Require.NotNull(exception, nameof(exception));

            return @this.ValueOrThrow(() => exception);
        }

        public static TSource ValueOrThrow<TSource>(this TSource? @this, Func<Exception> exceptionFactory)
            where TSource : struct
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            if (!@this.HasValue)
            {
                throw exceptionFactory();
            }

            return @this.Value;
        }

        public static Maybe<TSource> ToMaybe<TSource>(this TSource? @this) where TSource : struct
            => Maybe.Of(@this);
    }
}
