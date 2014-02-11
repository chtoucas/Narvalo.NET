// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;

    static class FuncExtensions
    {
        public static Func<Monad<T>, Monad<TResult>> Lift<T, TResult>(this Func<T, TResult> @this)
        {
            return m => m.Map(@this);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<TResult>>
            Lift<T1, T2, TResult>(this Func<T1, T2, TResult> @this)
        {
            return (m1, m2) => m1.Zip(m2, @this);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<TResult>>
            Lift<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this)
        {
            return (m1, m2, m3) => m1.Zip(m2, m3, @this);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<TResult>>
            Lift<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, TResult> @this)
        {
            return (m1, m2, m3, m4) => m1.Zip(m2, m3, m4, @this);
        }

        public static Func<Monad<T1>, Monad<T2>, Monad<T3>, Monad<T4>, Monad<T5>, Monad<TResult>>
            Lift<T1, T2, T3, T4, T5, TResult>(
            this Func<T1, T2, T3, T4, T5, TResult> @this)
        {
            return (m1, m2, m3, m4, m5) => m1.Zip(m2, m3, m4, m5, @this);
        }
    }
}
