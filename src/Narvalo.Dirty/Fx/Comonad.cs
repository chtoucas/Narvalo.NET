// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    static class Comonad
    {
        //// Extract (Counit, Coreturn)

        public static T Extract<T>(Comonad<T> monad)
        {
            return Comonad<T>.ε(monad);
        }

        //// Duplicate

        public static Comonad<Comonad<T>> Duplicate<T>(Comonad<T> monad)
        {
            return Comonad<T>.δ(monad);
        }
    }
}
