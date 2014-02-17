// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    public static partial class KuncExtensions
    {
        #region Basic Monad functions

        // [Haskell] =<<
        public static Monad<TResult> Invoke<TSource, TResult>(
            this Kunc<TSource, TResult> @this,
            Monad<TSource> monad)
        {
            return monad.Bind(@this);
        }

        // [Haskell] >=>
        public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
            this Kunc<TSource, TMiddle> @this,
            Kunc<TMiddle, TResult> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => @this.Invoke(_).Bind(kun);
        }

        // [Haskell] <=<
        public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
            this Kunc<TMiddle, TResult> @this,
            Kunc<TSource, TMiddle> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }

        #endregion
    }
}
