// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using Narvalo;
    using Narvalo.Fx;

    public static partial class KuncExtensions
    {
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
            => predicate ? @this : Stubs.Noop;

        public static Kunc<TSource, Unit> Filter<TSource>(this Kunc<TSource, Unit> @this, bool predicate)
            => predicate ? @this : Stubs<TSource>.Ignore;
    }
}
