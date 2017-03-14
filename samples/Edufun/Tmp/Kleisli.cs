// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Tmp
{
    using Narvalo;

    public delegate Prototype<T> Kunc<T>();

    public delegate Prototype<TResult> Kunc<in T, TResult>(T arg);

    public delegate Prototype<TResult> Kunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);

    public delegate T Cokunc<T>(Comonad<T> arg);

    public delegate TResult Cokunc<T, out TResult>(Comonad<T> arg);

    public static class Kleisli
    {
        // [Haskell] >=>
        public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
            this Kunc<TSource, TMiddle> @this,
            Kunc<TMiddle, TResult> kun)
        {
            Require.NotNull(@this, nameof(@this));

            return arg => @this.Invoke(arg).Bind(_ => kun(_));
        }

        // [Haskell] <=<
        public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
            this Kunc<TMiddle, TResult> @this,
            Kunc<TSource, TMiddle> kun)
        {
            Require.NotNull(kun, nameof(kun));

            return arg => kun.Invoke(arg).Bind(_ => @this(_));
        }
    }
}
