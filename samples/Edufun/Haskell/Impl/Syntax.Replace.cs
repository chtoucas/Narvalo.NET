// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    public partial class Functor<T>
    {
        // [GHC.Base] (<$) = fmap . const
        public Functor<TResult> Replace<TResult>(TResult other) => Select(_ => other);
    }

    public partial class Applicative<T>
    {
        // See Functor<T>.Replace()
        public Applicative<TResult> Replace<TResult>(TResult other) => Select(_ => other);
    }
}
