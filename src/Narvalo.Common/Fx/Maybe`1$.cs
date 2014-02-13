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
        #region Conditional execution of monadic expressions

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "Only here to have a complete Monad implementation.")]
        public static Maybe<Unit> Guard<TSource>(this Maybe<TSource> @this, bool predicate)
        {
            return predicate ? Maybe.Unit : Maybe.None;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this",
            Justification = "Only here to have a complete Monad implementation.")]
        public static Maybe<Unit> When<TSource>(this Maybe<TSource> @this, bool predicate, Action action)
        {
            Require.NotNull(action, "action");

            if (predicate) {
                action.Invoke();
            }

            return Maybe.Unit;
        }

        public static Maybe<Unit> Unless<TSource>(this Maybe<TSource> @this, bool predicate, Action action)
        {
            return When(@this, !predicate, action);
        }

        #endregion

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

        public static Maybe<TResult> Zip<T1, T2, T3, TResult>(
             this Maybe<T1> @this,
             Maybe<T2> second,
             Maybe<T3> third,
             Func<T1, T2, T3, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.IsSome && second.IsSome && third.IsSome
                ? Maybe.Create(resultSelector.Invoke(@this.Value, second.Value, third.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Zip<T1, T2, T3, T4, TResult>(
              this Maybe<T1> @this,
              Maybe<T2> second,
              Maybe<T3> third,
              Maybe<T4> fourth,
              Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.IsSome && second.IsSome && third.IsSome && fourth.IsSome
                ? Maybe.Create(resultSelector.Invoke(@this.Value, second.Value, third.Value, fourth.Value))
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
             this Maybe<T1> @this,
             Maybe<T2> second,
             Maybe<T3> third,
             Maybe<T4> fourth,
             Maybe<T5> fifth,
             Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");

            return @this.IsSome && second.IsSome && third.IsSome && fourth.IsSome && fifth.IsSome
                ? Maybe.Create(resultSelector.Invoke(@this.Value, second.Value, third.Value, fourth.Value, fifth.Value))
                : Maybe<TResult>.None;
        }

        #endregion

        #region MonadZero

        public static Maybe<TSource> Run<TSource>(this Maybe<TSource> @this, Action<TSource> action)
        {
            return OnSome(@this, action);
        }

        public static Maybe<TResult> Then<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? other : Maybe<TResult>.None;
        }

        #endregion

        public static Maybe<TSource> OnZero<TSource>(this Maybe<TSource> @this, Action action)
        {
            return OnNone(@this, action);
        }

        public static Maybe<TResult> Otherwise<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? Maybe<TResult>.None : other;
        }

        public static Maybe<TResult> Coalesce<TSource, TResult>(
            this Maybe<TSource> @this,
            Maybe<TResult> whenSome,
            Maybe<TResult> whenNone)
        {
            Require.Object(@this);

            return @this.IsSome ? whenSome : whenNone;
        }

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
