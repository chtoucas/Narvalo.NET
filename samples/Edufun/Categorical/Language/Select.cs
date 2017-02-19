// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Functor<T>
    {
        Functor<TResult> IFunctor<T>.Select<TResult>(Func<T, TResult> selector)
            => Select(selector);
    }

    public partial class Applicative<T>
    {
        // GHC.Base: liftA f a = pure f <*> a
        // fmap f x = pure f <*> x
        public Applicative<TResult> Select<TResult>(Func<T, TResult> selector)
            => Gather(Applicative.Of(selector));
    }

    public partial class Monad<T>
    {
        Monad<TResult> IMonad<T>.Select<TResult>(Func<T, TResult> selector)
            => Select(selector);
    }
}