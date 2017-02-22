// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Rules
{
    using System;

    // If an applicative functor is also a monad, it should satisfy:
    // - pure = return
    // - (<*>) = ap
    public static class ApplicativeRules
    {
        // pure id <*> v = v
        public static bool Identity<X>(Applicative<X> me)
        {
            Func<X, X> id = _ => _;

            return me.Gather(Applicative.Of(id)) == me;
        }

        // pure (.) <*> u <*> v <*> w = u <*> (v <*> w)
        public static bool Composition()
        {
            return true;
        }

        // pure f <*> pure x = pure (f x)
        public static bool Homomorphism<X, Y>(Func<X, Y> f, X value)
        {
            return Applicative.Of(value).Gather(Applicative.Of(f))
                == Applicative.Of(f(value));
        }

        // u <*> pure y = pure ($ y) <*> u
        public static bool Interchange()
        {
            throw new NotImplementedException();
        }
    }
}
