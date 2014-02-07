// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

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

        public static Maybe<TResult> Then<T, TResult>(this Maybe<T> @this, Maybe<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
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
    }
}
