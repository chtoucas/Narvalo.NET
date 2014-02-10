// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class MaybeExtensions
    {
        //// Zip

        public static Maybe<TResult> Zip<TFirst, TSecond, TResult>(
            this Maybe<TFirst> @this,
            Maybe<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.IsSome && second.IsSome
                ? Maybe.Create(resultSelector.Invoke(@this.Value, second.Value))
                : Maybe<TResult>.None;
        }

        //// Run

        public static Maybe<TSource> Run<TSource>(this Maybe<TSource> @this, Action<TSource> action)
        {
            return OnSome(@this, action);
        }

        //// Then

        public static Maybe<TResult> Then<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? other : Maybe<TResult>.None;
        }

        #region Optional methods.

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

        //// Coalesce

        public static Maybe<TResult> Coalesce<TSource, TResult>(
            this Maybe<TSource> @this,
            Maybe<TResult> whenSome,
            Maybe<TResult> whenNone)
        {
            Require.Object(@this);

            return @this.IsSome ? whenSome : whenNone;
        }

        //// Otherwise

        public static Maybe<TResult> Otherwise<TSource, TResult>(this Maybe<TSource> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.IsSome ? Maybe<TResult>.None : other;
        }

        //// OnZero

        public static Maybe<TSource> OnZero<TSource>(this Maybe<TSource> @this, Action action)
        {
            return OnNone(@this, action);
        }

        #endregion
    }
}
