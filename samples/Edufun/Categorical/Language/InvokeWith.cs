// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class FunctorSyntax
    {
        // (<$>) = fmap
        public Functor<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Functor<T> value)
            => value.Select(selector);
    }

    public partial class Applicative
    {
        // Data.Functor: (<$>) = fmap
        public Applicative<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Applicative<T> value)
            => value.Select(selector);
    }

    public partial class Monad
    {
        public Monad<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Monad<T> value)
            => value.Select(selector);
    }
}