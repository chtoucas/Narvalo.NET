// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using Narvalo.Fx;

    public static partial class Monad
    {
        private static readonly Monad<Unit> s_Unit = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit => s_Unit;

        // [Haskell] return
        public static Monad<T> Return<T>(T value) => Monad<T>.η(value);
    }
}
