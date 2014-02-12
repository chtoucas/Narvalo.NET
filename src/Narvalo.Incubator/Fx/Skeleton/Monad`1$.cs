// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    static partial class MonadExtensions
    {
        #region MonadZero (?)

        public static Monad<TSource> Run<TSource>(this Monad<TSource> @this, Kunc<TSource, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (!@this.IsZero) {
                @this.Bind(action);
            }

            return @this;
        }

        public static Monad<TSource> OnZero<TSource>(this Monad<TSource> @this, Kunc<Unit, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            if (@this.IsZero) {
                action.Invoke(Unit.Single);
            }

            return @this;
        }

        public static Monad<TResult> Then<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return !@this.IsZero ? other : Monad<TResult>.Zero;
        }

        public static Monad<TResult> Otherwise<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return !@this.IsZero ? Monad<TResult>.Zero : other;
        }

        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);

            return !@this.IsZero ? then : otherwise;
        }

        #endregion
    }
}
