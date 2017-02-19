// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Functor<T> : IFunctor<T>, IFunctorSyntax<T>
    {
        private readonly IFunctor<T> _me;

        public Functor(IFunctor<T> me) { _me = me; }

        public Functor<TResult> Select<TResult>(Func<T, TResult> selector) => _me.Select(selector);
        //public IFunctor<TResult> Select<TResult>(Func<T, TResult> selector)
        //{
        //    throw new FakeClassException();
        //}
    }

    public static class Functor
    {
        private static readonly IFunctorSyntax s_Impl = new FunctorSyntax();

        public static Functor<TResult> Inject<T, TResult>(TResult other, Functor<T> value)
            => s_Impl.Inject(other, value);

        public static Functor<TResult> InvokeWith<T, TResult>(Func<T, TResult> selector, Functor<T> value)
            => s_Impl.InvokeWith(selector, value);
    }

    public partial class FunctorSyntax : IFunctorSyntax { }
}
