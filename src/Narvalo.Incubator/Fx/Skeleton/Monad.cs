// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Skeleton
{
    static class Monad
    {
        static readonly Monad<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);
        static readonly Monad<Unit> Zero_ = Monad<Unit>.Zero;

        public static Monad<Unit> Unit { get { return Unit_; } }

        // WARNING: Only for Monads with a Zero
        public static Monad<Unit> Zero { get { return Zero_; } }

        public static Monad<T> Return<T>(T value)
        {
            return Monad<T>.η(value);
        }

        public static Monad<T> Flatten<T>(Monad<Monad<T>> square)
        {
            return Monad<T>.μ(square);
        }
    }
}
