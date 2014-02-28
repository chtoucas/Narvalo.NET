// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using Narvalo.Fx;

    public static class Identity
    {
        static readonly Identity<Unit> Unit_ = Return(Narvalo.Fx.Unit.Single);

        public static Identity<Unit> Unit { get { return Unit_; } }

        public static Identity<T> Return<T>(T value)
        {
            return Identity<T>.η(value);
        }

        #region Generalisations of list functions

        public static Identity<T> Flatten<T>(Identity<Identity<T>> square)
        {
            return square.Value;
        }

        #endregion

        public static T Extract<T>(Identity<T> monad)
        {
            return monad.Value;
        }

        public static Identity<Identity<T>> Duplicate<T>(Identity<T> monad)
        {
            return Identity<T>.δ(monad);
        }
    }
}
