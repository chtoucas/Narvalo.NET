// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Tmp
{
    using System;

    using Narvalo;
    using Narvalo.Fx;

    public delegate Monad<T> Kunc<T>();

    public delegate Monad<TResult> Kunc<in T, TResult>(T arg);

    public delegate Monad<TResult> Kunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);

    public delegate T Cokunc<T>(Comonad<T> arg);

    public delegate TResult Cokunc<T, out TResult>(Comonad<T> arg);

    public static class Kleisli
    {
        public static readonly Kunc<Unit, Unit> Noop = _ => Monad.Unit;

        public static Kunc<T, Unit> Ignore<T>() => _ => Monad.Unit;

        public static Kunc<Unit, Unit> ToKunc(this Action @this)
            => _ => { @this.Invoke(); return Monad.Unit; };

        public static Kunc<TSource, Unit> ToKunc<TSource>(this Action<TSource> @this)
            => _ => { @this.Invoke(_); return Monad.Unit; };

        // [Haskell] =<<
        public static Monad<TResult> Invoke<TSource, TResult>(
           this Kunc<TSource, TResult> @this,
           Monad<TSource> monad)
            => monad.Bind(@this);

        // [Haskell] >=>
        public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
            this Kunc<TSource, TMiddle> @this,
            Kunc<TMiddle, TResult> kun)
        {
            Require.NotNull(@this, nameof(@this));

            return _ => @this.Invoke(_).Bind(kun);
        }

        // [Haskell] <=<
        public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
            this Kunc<TMiddle, TResult> @this,
            Kunc<TSource, TMiddle> kun)
        {
            Expect.NotNull(@this);
            Require.NotNull(kun, nameof(kun));

            return _ => kun.Invoke(_).Bind(@this);
        }

        public static Kunc<Unit, Unit> Filter(this Kunc<Unit, Unit> @this, bool predicate)
            => predicate ? @this : Noop;

        public static Kunc<TSource, Unit> Filter<TSource>(this Kunc<TSource, Unit> @this, bool predicate)
            => predicate ? @this : Ignore<TSource>();
    }
}
