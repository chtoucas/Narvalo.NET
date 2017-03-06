// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun
{
    using System;

    using Narvalo;
    using Narvalo.Applicative;

    /// <summary>
    /// The Func monad :-)
    /// </summary>
    public static partial class Func
    {
        public static Func<T> Return<T>(T value)
        {
            return () => value;
        }

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.Invoke();
        }

        public static Action Where(this Action @this, bool predicate)
            => predicate ? @this : Stubs.Noop;

        public static Action<TSource> Where<TSource>(this Action<TSource> @this, bool predicate)
            => predicate ? @this : Stubs<TSource>.Ignore;

        public static Func<TResult> Bind<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, Func<TResult>> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return () => selector.Invoke(@this.Invoke());
        }

        #region Extensions for Func<Nullable<T>>

        public static TResult? Invoke<TSource, TResult>(
            this Func<TSource, TResult?> @this,
            TSource? value)
            where TSource : struct
            where TResult : struct
            => value.Bind(@this);

        public static Func<TSource, TResult?> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, TMiddle?> @this,
            Func<TMiddle, TResult?> selector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));

            return _ => @this.Invoke(_).Bind(selector);
        }

        public static Func<TSource, TResult?> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, TResult?> @this,
            Func<TSource, TMiddle?> selector)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return _ => selector.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
