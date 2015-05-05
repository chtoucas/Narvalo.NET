// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Monads
{
    public static class Stubs<T>
    {
        private static readonly Kunc<T, Unit> s_Ignore = _ => Monad.Unit;

        public static Kunc<T, Unit> Ignore { get { return s_Ignore; } }
    }
}
