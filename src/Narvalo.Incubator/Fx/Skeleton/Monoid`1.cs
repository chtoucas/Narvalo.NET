// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define MONAD_VIA_BIND

namespace Narvalo.Fx.Skeleton
{
    using System;

    sealed class Monoid<T>
    {
        // mzero :: m a
        public static Monoid<T> Zero { get { throw new NotImplementedException(); } }

        // mplus :: m a -> m a -> m a
        public Monoid<T> Compose(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
