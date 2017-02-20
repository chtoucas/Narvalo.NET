// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;

    public partial class Monad<T>
    {
        public Monad<TResult> Gather<TResult>(Monad<Func<T, TResult>> applicative)
        {
            throw new NotImplementedException();
        }
    }
}