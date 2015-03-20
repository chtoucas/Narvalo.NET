// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Extensions
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extensions methods for <see cref="Func{T}"/>.
    /// </summary>
    public static partial class FuncExtensions
    {
        public static Func<TResult> Bind<TSource, TResult>(this Func<TSource> @this, Func<TSource, Func<TResult>> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");

            return selector.Invoke(@this.Invoke());
        }

        public static Func<TResult> Select<TSource, TResult>(this Func<TSource> @this, Func<TSource, TResult> selector)
        {
            Require.Object(@this);
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Func<TResult>>() != null);

            return () => selector.Invoke(@this.Invoke());
        }
    }
}
