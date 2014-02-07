// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using Narvalo.Linq;

    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static class MaybeExtensions
    {
        //// Match

        public static TResult Match<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector,
            TResult defaultValue)
        {
            Require.Object(@this);

            return @this.Map(selector).ValueOrElse(defaultValue);
        }

        public static TResult Match<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, TResult> selector,
            Func<TResult> defaultValueFactory)
        {
            Require.Object(@this);

            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

        //// Then

        public static Maybe<TResult> ThenOtherwise<T, TResult>(
            this Maybe<T> @this, 
            Maybe<TResult> whenSome,
            Maybe<TResult> whenNone)
        {
            Require.Object(@this);

            return @this.IsSome ? whenSome : whenNone;
        }

        //// Then

        public static Maybe<TResult> Then<T, TResult>(this Maybe<T> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? other : Maybe<TResult>.None;
        }

        //// Otherwise

        public static Maybe<TResult> Otherwise<T, TResult>(this Maybe<T> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? Maybe<TResult>.None : other;
        }

        //// OnSome

        public static Maybe<T> OnSome<T>(this Maybe<T> @this, Action<T> action)
        {
            Require.Object(@this);

            Require.NotNull(action, "action");

            if (@this.IsSome) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        //// OnNone

        public static Maybe<T> OnNone<T>(this Maybe<T> @this, Action action)
        {
            Require.Object(@this);

            Require.NotNull(action, "action");

            if (@this.IsNone) {
                action.Invoke();
            }

            return @this;
        }

        //// 

        public static T UnpackOrDefault<T>(this Maybe<T?> @this) where T : struct
        {
            return UnpackOrElse(@this, default(T));
        }

        public static T UnpackOrElse<T>(this Maybe<T?> @this, T defaultValue) where T : struct
        {
            return UnpackOrElse(@this, () => defaultValue);
        }

        public static T UnpackOrElse<T>(this Maybe<T?> @this, Func<T> defaultValueFactory) where T : struct
        {
            Require.Object(@this);

            return @this.ValueOrDefault() ?? defaultValueFactory.Invoke();
        }

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Exception exception) where T : struct
        {
            return UnpackOrThrow(@this, () => exception);
        }

        public static T UnpackOrThrow<T>(this Maybe<T?> @this, Func<Exception> exceptionFactory) where T : struct
        {
            return ToNullable(@this).OnNothing(() => { exceptionFactory.Invoke(); }).Value;
        }

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
        {
            Require.Object(@this);

            return @this.ValueOrDefault();
        }

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
        {
            Require.Object(@this);

            return @this.IsSome ? (T?)@this.Value : null;
        }

        //// Conversions Operators

        public static Maybe<TResult> Cast<TSource, TResult>(this Maybe<TSource> @this) where TSource : TResult
        {
            Require.Object(@this);

            return from _ in @this select (TResult)_;
        }
    }
}
