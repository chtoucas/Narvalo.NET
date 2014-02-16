// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;

    public static class FuncMonad
    {
        public static Func<T> Return<T>(T value)
        {
            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            return () => square.Invoke().Invoke();
            //return square.Bind(_ => _);
        }
    }

    public static partial class FuncExtensions
    {
        #region Monad

        public static Func<TResult> Bind<TSource, TResult>(this Func<TSource> @this, Func<TSource, Func<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return () => selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Then<TSource, TResult>(this Func<TSource> @this, Func<TResult> other)
        {
            return other;
        }

        #endregion

        #region Basic Monad functions for Identity

        public static Func<TSource, Identity<TResult>> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, Identity<TMiddle>> @this,
            Func<TMiddle, Identity<TResult>> kun)
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }

        public static Func<TSource, Identity<TResult>> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, Identity<TResult>> @this,
            Func<TSource, Identity<TMiddle>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
