// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public static partial class FuncExtensions
    {
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
