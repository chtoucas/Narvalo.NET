// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using Narvalo.Fx;

    public static class Stubs
    {
        private static readonly Kunc<Unit, Unit> s_Noop = _ => Monad.Unit;

        public static Kunc<Unit, Unit> Noop => s_Noop;
    }
}
