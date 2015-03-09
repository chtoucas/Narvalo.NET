// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads
{
    using Narvalo.Fx;

    public static partial class KuncExtensions
    {
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
