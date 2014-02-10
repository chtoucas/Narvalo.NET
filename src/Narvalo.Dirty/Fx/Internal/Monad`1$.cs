// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    static partial class MonadExtensions
    {
        //// Zip

        [Obsolete]
        public static Monad<TResult> Zip<T1, T2, T3, TResult>(
             this Monad<T1> @this,
             Monad<T2> monad2,
             Monad<T3> monad3,
             Func<T1, T2, T3, TResult> resultSelector)
        {
            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, (t2, t3) => resultSelector.Invoke(t1, t2, t3));

            return @this.Bind(g);
        }

        [Obsolete]
        public static Monad<TResult> Zip<T1, T2, T3, T4, TResult>(
              this Monad<T1> @this,
              Monad<T2> monad2,
              Monad<T3> monad3,
              Monad<T4> monad4,
              Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, monad4, (t2, t3, t4) => resultSelector.Invoke(t1, t2, t3, t4));

            return @this.Bind(g);
        }

        [Obsolete]
        public static Monad<TResult> Zip<T1, T2, T3, T4, T5, TResult>(
             this Monad<T1> @this,
             Monad<T2> monad2,
             Monad<T3> monad3,
             Monad<T4> monad4,
             Monad<T5> monad5,
             Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            Kunc<T1, TResult> g
                = t1 => monad2.Zip(monad3, monad4, monad5, (t2, t3, t4, t5) => resultSelector.Invoke(t1, t2, t3, t4, t5));

            return @this.Bind(g);
        }
    }
}
