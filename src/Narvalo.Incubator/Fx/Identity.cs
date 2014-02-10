// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Identity
    {
        static readonly Identity<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Identity<Unit> Unit { get { return Unit_; } }

        //// Return

        public static Identity<T> Return<T>(T value)
        {
            return Identity<T>.η(value);
        }

        //// Join

        public static Identity<T> Join<T>(Identity<Identity<T>> square)
        {
            return Identity<T>.μ(square);
        }

        //// Extract

        public static T Extract<T>(Identity<T> monad)
        {
            return Identity<T>.ε(monad);
        }

        //// Duplicate

        public static Identity<Identity<T>> Duplicate<T>(Identity<T> monad)
        {
            return Identity<T>.δ(monad);
        }
    }
}
