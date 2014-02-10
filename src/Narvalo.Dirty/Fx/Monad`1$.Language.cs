// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    static partial class MonadExtensions
    {
        //// Zip

        public static Monad<TResult> Zip<TFirst, TSecond, TResult>(
            this Monad<TFirst> @this,
            Monad<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            return @this.Bind(m1 => second.Map(m2 => resultSelector.Invoke(m1, m2)));
        }

        //// Run

        public static Monad<TSource> Run<TSource>(this Monad<TSource> @this, Action<TSource> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Bind(action.ToKunc()).Then(@this);
        }

        //// Then

        public static Monad<TResult> Then<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }

        #region Optional methods.

        //// Match (if ValueOrElse())

        public static TResult Match<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, TResult> selector,
            TResult defaultValue)
        {
            Require.Object(@this);

            return @this.Map(selector).ValueOrElse(defaultValue);
        }

        public static TResult Match<TSource, TResult>(
            this Monad<TSource> @this,
            Func<TSource, TResult> selector,
            Func<TResult> defaultValueFactory)
        {
            Require.Object(@this);
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.Match(selector, defaultValueFactory.Invoke());
        }

        //// Coalesce (if Zero)

        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);

            return @this.Then(then).Otherwise(otherwise);
        }

        //// Otherwise (if Zero)

        public static Monad<TResult> Otherwise<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => Monad.Zero).Bind(_ => other);
        }

        //// OnZero (if Zero)

        public static Monad<TSource> OnZero<TSource>(this Monad<TSource> @this, Action action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            return @this.Otherwise(Monad.Unit).Bind(action.ToKunc()).Then(@this);
        }

        #endregion
    }
}
