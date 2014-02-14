// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    static class KuncExtensions
    {
        #region Basic Monad functions

        // [Haskell] >=>
        // Left-to-right Kleisli composition of monads.
        public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
            this Kunc<TSource, TMiddle> @this,
            Kunc<TMiddle, TResult> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => @this.Invoke(_).Bind(kun);
        }

        // [Haskell] <=<
        // Right-to-left Kleisli composition of monads. (>=>), with the arguments flipped.
        public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
            this Kunc<TMiddle, TResult> @this,
            Kunc<TSource, TMiddle> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return _ => kun.Invoke(_).Bind(@this);
        }

        #endregion

        public static Kunc<Unit, Unit> Filter(this Kunc<Unit, Unit> @this, bool predicate)
        {
            return predicate ? @this : Stubs.Noop;
        }

        public static Kunc<TSource, Unit> Filter<TSource>(this Kunc<TSource, Unit> @this, bool predicate)
        {
            return predicate ? @this : Stubs<TSource>.Ignore;
        }
    }
}
