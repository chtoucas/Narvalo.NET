// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Edu.Monads
{
    using Narvalo.Fx;

    public static partial class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Monad<Unit> Unit { get { return Unit_; } }

        // [Haskell] return
        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }
    }
}
