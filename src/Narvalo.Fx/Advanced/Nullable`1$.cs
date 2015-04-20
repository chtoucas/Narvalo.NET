﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Advanced
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="Nullable{T}"/>.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>What's not to be found here:</listheader>
    /// <item><description><c>Return</c> is simply casting: <c>(T?)value</c>.</description></item>
    /// <item><description><c>Nullable</c> does not support the <c>Join</c> operation; there is no
    /// <c>Nullable&lt;Nullable&lt;T&gt;&gt;</c>.</description></item>
    /// <item><description><c>Zero</c> is <see langword="null"/>.</description></item>
    /// </list>
    /// </remarks>
    public static partial class NullableExtensions
    {
        #region Monad

        public static TResult? Bind<TSource, TResult>(this TSource? @this, Func<TSource, TResult?> selector)
            where TSource : struct
            where TResult : struct
        {
            Require.NotNull(selector, "selector");

            return @this.HasValue ? selector.Invoke(@this.Value) : null;
        }

        #endregion

        #region Basic Monad functions

        public static TResult? Select<TSource, TResult>(this TSource? @this, Func<TSource, TResult> selector)
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
            Require.NotNull(resultSelector, "resultSelector");

            return @this.HasValue && second.HasValue
                ? (TResult?)resultSelector.Invoke(@this.Value, second.Value)
                : null;
        }

        #endregion

        #region Query Expression Pattern

        public static TSource? Where<TSource>(
            this TSource? @this,
            Func<TSource, bool> predicate)
            where TSource : struct
        {
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? @this : null);
        }

        public static TResult? SelectMany<TSource, TMiddle, TResult>(
            this TSource? @this,
            Func<TSource, TMiddle?> valueSelector,
            Func<TSource, TMiddle, TResult> resultSelector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(valueSelector, "valueSelector");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.Bind(_ => valueSelector(_).Select(middle => resultSelector(_, middle)));
        }

        #endregion

        #region Non-standard extensions

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
            Contract.Requires(predicate != null);

            return Coalesce(@this, predicate, other, null);
        }

        public static TResult? Otherwise<TSource, TResult>(
            this TSource? @this,
            Func<TSource, bool> predicate,
            TResult? other)
            where TSource : struct
            where TResult : struct
        {
            Contract.Requires(predicate != null);

            return Coalesce(@this, predicate, null, other);
        }

        public static void Invoke<TSource>(this TSource? @this, Action<TSource> action)
            where TSource : struct
        {
            Require.NotNull(action, "action");

            if (@this.HasValue)
            {
                action.Invoke(@this.Value);
            }
        }

        public static void OnNull<TSource>(this TSource? @this, Action action)
            where TSource : struct
        {
            Require.NotNull(action, "action");

            if (!@this.HasValue)
            {
                action.Invoke();
            }
        }

        public static void Apply<TSource>(this TSource? @this, Action<TSource> action, Action caseNull)
            where TSource : struct
        {
            Require.NotNull(action, "action");
            Require.NotNull(caseNull, "caseNull");

            if (@this.HasValue)
            {
                action.Invoke(@this.Value);
            }
            else
            {
                caseNull.Invoke();
            }
        }

        #endregion

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

            if (!@this.HasValue)
            {
                throw exceptionFactory.Invoke();
            }

            return @this.Value;
        }

        public static Maybe<TSource> ToMaybe<TSource>(this TSource? @this) where TSource : struct
        {
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return Maybe.Of(@this);
        }

        public static void OnValue<TSource>(this TSource? @this, Action<TSource> action)
            where TSource : struct
        {
            Contract.Requires(action != null);

            @this.Invoke(action);
        }
    }
}