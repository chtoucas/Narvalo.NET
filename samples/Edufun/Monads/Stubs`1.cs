// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using Narvalo.Fx;

    public static class Stubs<T>
    {
        private static readonly Kunc<T, Unit> s_Ignore = _ => Monad.Unit;

        public static Kunc<T, Unit> Ignore => s_Ignore;
    }
}
