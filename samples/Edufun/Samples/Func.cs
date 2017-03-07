// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Samples
{
    using System;

    using Narvalo;
    using Narvalo.Applicative;

    public static partial class Func
    {
        public static Func<T> Return<T>(T value) => () => value;

        public static Func<T> Flatten<T>(Func<Func<T>> square)
        {
            Require.NotNull(square, nameof(square));

            return square.Invoke();
        }

        public static Func<TResult> Bind<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, Func<TResult>> binder)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(binder, nameof(binder));

            return binder(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(
            this Func<TSource> @this,
            Func<TSource, TResult> selector)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(selector, nameof(selector));

            return () => selector(@this.Invoke());
        }

        public static Action Where(this Action @this, bool predicate)
            => predicate ? @this : Stubs.Noop;

        public static Action<TSource> Where<TSource>(this Action<TSource> @this, bool predicate)
            => predicate ? @this : Stubs<TSource>.Ignore;
    }
}
