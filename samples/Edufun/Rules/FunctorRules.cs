// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Rules
{
    using System;

    public static class FunctorRules
    {
        // First law: the identity map is a fixed point for Select.
        // fmap id  ==  id
        public static bool FirstLaw<X>(Prototype<X> me)
        {
            Func<X, X> id = _ => _;
            Func<Prototype<X>, Prototype<X>> idM = _ => _;

            return me.Select(id) == idM.Invoke(me);
        }

        // Second law: Select preserves the composition operator.
        // fmap (f . g)  ==  fmap f . fmap g
        public static bool SecondLaw<X, Y, Z>(Prototype<X> me, Func<Y, Z> f, Func<X, Y> g)
        {
            return me.Select(_ => f(g(_))) == me.Select(g).Select(f);
        }
    }
}
