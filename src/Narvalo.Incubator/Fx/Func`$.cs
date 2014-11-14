// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static partial class FuncExtensions
    {
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

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Func<TResult> Then<TSource, TResult>(this Func<TSource> @this, Func<TResult> other)
        {
            return other;
        }

        #region Basic Monad functions for Nullable

        public static Func<TSource, TResult?> Compose<TSource, TMiddle, TResult>(
            this Func<TSource, TMiddle?> @this,
            Func<TMiddle, TResult?> kun)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.Object(@this);

            return _ => @this.Invoke(_).Bind(kun);
        }

        public static Func<TSource, TResult?> ComposeBack<TSource, TMiddle, TResult>(
            this Func<TMiddle, TResult?> @this,
            Func<TSource, TMiddle?> kun)
            where TSource : struct
            where TMiddle : struct
            where TResult : struct
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
