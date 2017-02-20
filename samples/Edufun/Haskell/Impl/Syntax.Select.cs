// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;

    public partial class Applicative<T>
    {
        // [GHC.Base] liftA f a = pure f <*> a
        // [Control.Applicative] fmap f x = pure f <*> x
        public Applicative<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Applicative.Of(selector));
    }
}