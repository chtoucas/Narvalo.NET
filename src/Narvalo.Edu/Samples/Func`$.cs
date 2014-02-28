// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "this")]
        public static Func<TResult> Then<TSource, TResult>(this Func<TSource> @this, Func<TResult> other)
        {
            return other;
        }

        #endregion
    }
}
