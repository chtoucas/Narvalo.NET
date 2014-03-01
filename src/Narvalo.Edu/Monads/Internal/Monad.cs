// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads.Internal
{
    using Narvalo.Fx;

    static partial class Monad
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
