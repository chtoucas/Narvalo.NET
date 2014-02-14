// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides extension methods for <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static partial class MaybeExtensions
    {
        #region Monadic lifting operators

        public static Maybe<TResult> Zip<TFirst, TSecond, TResult>(
            this Maybe<TFirst> @this,
            Maybe<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");

            return @this.IsSome && second.IsSome
                ? Maybe.Create(resultSelector.Invoke(@this.Value, second.Value))
                : Maybe<TResult>.None;
        }

        #endregion

        #region Additional methods

        public static Maybe<TResult> Coalesce<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> then,
            Maybe<TResult> otherwise)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Bind(_ => predicate.Invoke(_) ? then : otherwise);
        }

        public static Maybe<TResult> Then<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> other)
        {
            return Coalesce(@this, predicate, other, Maybe<TResult>.None);
        }

        public static Maybe<TResult> Otherwise<TSource, TResult>(
            this Maybe<TSource> @this,
            Func<TSource, bool> predicate,
            Maybe<TResult> other)
        {
            return Coalesce(@this, predicate, Maybe<TResult>.None, other);
        }

        public static Maybe<Unit> Run<TSource>(this Maybe<TSource> @this, Action<TSource> action)
        {
            OnSome(@this, action);

            return Maybe.Unit;
        }

        #endregion

        //// OnSome & OnNone

        public static Maybe<TSource> OnSome<TSource>(this Maybe<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsSome) {
                action.Invoke(@this.Value);
            }

            return @this;
        }

        public static Maybe<TSource> OnNone<TSource>(this Maybe<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsNone) {
                action.Invoke();
            }

            return @this;
        }

        //// ToNullable

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

        //// UnpackOr...

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
            return ToNullable(@this).OnNull(() => { exceptionFactory.Invoke(); }).Value;
        }
    }
}
