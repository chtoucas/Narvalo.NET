// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;

    static partial class MonadExtensions
    {
        #region MonadZero

        public static Monad<Unit> Run<TSource>(this Monad<TSource> @this, Kunc<TSource, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

           return  @this.Bind(_ => action.Invoke(_));
        }

        public static Monad<TResult> Then<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            return @this.Bind(_ => other);
        }

        #endregion

        public static Monad<Unit> OnZero<TSource>(this Monad<TSource> @this, Kunc<Unit, Unit> action)
        {
            Require.Object(@this);
            Require.NotNull(action, "action");

            throw new NotImplementedException();
        }

        public static Monad<TResult> Otherwise<TSource, TResult>(this Monad<TSource> @this, Monad<TResult> other)
        {
            Require.Object(@this);

            throw new NotImplementedException();
        }

        public static Monad<TResult> Coalesce<TSource, TResult>(
            this Monad<TSource> @this,
            Monad<TResult> then,
            Monad<TResult> otherwise)
        {
            Require.Object(@this);

            throw new NotImplementedException();
        }
    }
}
