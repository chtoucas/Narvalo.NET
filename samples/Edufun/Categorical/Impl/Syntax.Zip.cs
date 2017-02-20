// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Impl
{
    using System;

    public partial class Applicative<T>
    {
        // [GHC.Base] liftA2 f a b = fmap f a <*> b
        public Applicative<TResult> Zip<TSecond, TResult>(
            Applicative<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Func<T, Func<TSecond, TResult>> selector
                = (T x) => (TSecond y) => resultSelector.Invoke(x, y);

            return second.Gather(Select(selector));
        }

        // [GHC.Base] liftA3 f a b c = fmap f a <*> b <*> c
        public Applicative<TResult> Zip<T2, T3, TResult>(
            Applicative<T2> second,
            Applicative<T3> third,
            Func<T, T2, T3, TResult> resultSelector)
        {
            Func<T, Func<T2, Func<T3, TResult>>> selector
                = (T x) => (T2 y) => (T3 z) => resultSelector.Invoke(x, y, z);

            Applicative<Func<T3, TResult>> app = second.Gather(Select(selector));

            return third.Gather(app);
        }
    }
}
