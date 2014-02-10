// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class FuncExtensions
    {
        ////// Compose (Lazy)

        //public static Func<TSource, Lazy<TResult>> Compose<TSource, TMiddle, TResult>(
        //    this Func<TSource, Lazy<TMiddle>> @this,
        //    Func<TMiddle, Lazy<TResult>> kun)
        //{
        //    Require.Object(@this);

        //    return _ => @this.Invoke(_).Bind(kun);
        //}
    }
}
