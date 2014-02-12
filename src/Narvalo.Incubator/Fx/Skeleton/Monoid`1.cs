// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    using System;

    sealed class Monoid<T>
    {
        // [Haskell] mempty
        public static Monoid<T> Empty { get { throw new NotImplementedException(); } }

        // [Haskell] mappend
        public Monoid<T> Append(Monoid<T> other)
        {
            throw new NotImplementedException();
        }
    }
}
