// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads
{
    using Narvalo.Fx;

    public static class Stubs<T>
    {
        static readonly Kunc<T, Unit> Ignore_ = _ => Monad.Unit;

        public static Kunc<T, Unit> Ignore { get { return Ignore_; } }
    }
}
