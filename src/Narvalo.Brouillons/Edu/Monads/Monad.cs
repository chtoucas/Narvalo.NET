// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads
{
    using Narvalo.Fx;

    public static partial class Monad
    {
        private static readonly Monad<Unit> s_Unit = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit { get { return s_Unit; } }

        // [Haskell] return
        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }
    }
}
