// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define MONAD_VIA_BIND

namespace Narvalo.Fx.Skeleton
{
    using System;

    sealed class Monad<T>
    {
        #region Monad with a Zero

        // mzero :: m a
        public static Monad<T> Zero { get { throw new NotImplementedException(); } }

        public bool IsZero { get { throw new NotImplementedException(); } }

        #endregion

        // >>= :: m a -> (a -> m b) -> m b
        public Monad<TResult> Bind<TResult>(Kunc<T, TResult> kun)
        {
#if MONAD_VIA_BIND
            throw new NotImplementedException();
#else
            return Monad<TResult>.μ(Map(_ => kun.Invoke(_)));
#endif
        }

        // >> :: m a -> m b -> m b
        // liftM :: Monad m => (a -> b) -> (m a -> m b)
        // fmap :: Functor f => (a -> b) -> f a -> f b
        public Monad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if MONAD_VIA_BIND
            return Bind(_ => Monad<TResult>.η(fun.Invoke(_)));
#else
            throw new NotImplementedException();
#endif
        }

        // fail :: String -> m a
        public Monad<T> Otherwise()
        {
            throw new NotImplementedException();
        }

        // return :: Monad m => a -> m a
        internal static Monad<T> η(T value)
        {
            throw new NotImplementedException();
        }

        // join :: Monad m => m (m a) -> m a
        internal static Monad<T> μ(Monad<Monad<T>> square)
        {
#if MONAD_VIA_BIND
            return square.Bind(_ => _);
#else
            throw new NotImplementedException();
#endif
        }
    }
}
