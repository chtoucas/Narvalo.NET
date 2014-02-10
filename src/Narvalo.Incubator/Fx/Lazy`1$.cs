// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="System.Lazy{T}"/>.
    /// </summary>
    public static partial class LazyExtensions
    {
        ////// Bind

        //public static Lazy<TResult> Bind<TSource, TResult>(this Lazy<TSource> @this, Func<TSource, Lazy<TResult>> selector)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(selector, "selector");

        //    return Lazy.Create(() => selector.Invoke(@this.Value).Value);
        //}

        ////// Map

        //public static Lazy<TResult> Map<TSource, TResult>(this Lazy<TSource> @this, Func<TSource, TResult> selector)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(selector, "selector");

        //    return Lazy.Create(() => selector.Invoke(@this.Value));
        //}
    }
}
