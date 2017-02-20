// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Rules
{
    using System;

    using Edufun.Categorical.Impl;

    public static class FunctorRules
    {
        // First law: the identity map is a fixed point for Select.
        // fmap id  ==  id
        public static bool FirstLaw<X>(Functor<X> me)
        {
            Func<X, X> id = _ => _;
            Func<Functor<X>, Functor<X>> idM = _ => _;

            return me.Select(id) == idM.Invoke(me);
        }

        // Second law: Select preserves the composition operator.
        // fmap (f . g)  ==  fmap f . fmap g
        public static bool SecondLaw<X, Y, Z>(Functor<X> me, Func<Y, Z> f, Func<X, Y> g)
        {
            return me.Select(_ => f(g(_))) == me.Select(g).Select(f);
        }
    }
}
