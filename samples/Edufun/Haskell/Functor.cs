// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;

    using Narvalo.Fx;

    public partial class Functor<T> : IFunctor<T>
    {
        public Functor<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            throw new FakeClassException();
        }
    }

    public partial class Functor<T> : IFunctorSyntax<T>
    {
        // [GHC.Base] (<$) = fmap . const
        public Functor<TResult> Replace<TResult>(TResult other) => Select(_ => other);

        // [Data.Functor] void x = () <$ x
        public Functor<Unit> Skip() => Replace(Unit.Single);
    }

    public class Functor : IFunctorOperators
    {
        // ($>) = flip (<$)
        public Functor<TResult> Inject<TSource, TResult>(TResult other, Functor<TSource> value)
            => value.Replace(other);

        // (<$>) = fmap
        public Functor<TResult> InvokeWith<TSource, TResult>(Func<TSource, TResult> selector, Functor<TSource> value)
            => value.Select(selector);
    }
}
