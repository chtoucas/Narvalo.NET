// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    static class KuncExtensions
    {
        public static Kunc<TSource, TResult> Compose<TSource, TMiddle, TResult>(
            this Kunc<TSource, TMiddle> @this,
            Kunc<TMiddle, TResult> kun)
        {
            return _ => Monad.Compose(@this, kun, _);
        }

        public static Kunc<TSource, TResult> ComposeBack<TSource, TMiddle, TResult>(
            this Kunc<TMiddle, TResult> @this,
            Kunc<TSource, TMiddle> kun)
        {
            return _ => Monad.ComposeBack(@this, kun, _);
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
