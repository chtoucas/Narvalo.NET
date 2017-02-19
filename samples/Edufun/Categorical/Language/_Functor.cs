// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Functor : IFunctorGrammar { }

    public partial class Functor<T> : IFunctor<T>, IFunctorGrammar<T>
    {
        public Functor<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            throw new FakeClassException();
        }
    }
}
