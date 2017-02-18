// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun
{
    using System;

    internal interface MONAD<T>
    {
        #region Functor

        // Named "fmap", "liftA" or "<$>" (Applicative) in Haskell parlance.
        MONAD<TResult> Select<TResult>(Func<T, TResult> selector);

        #endregion

        #region Applicative

        // Named "return" or "pure" (Applicative) in Haskell parlance.
        MONAD<T> Of(T value);

        // Named ">>=" in Haskell parlance.
        MONAD<TResult> Bind<TResult>(Func<T, MONAD<TResult>> selector);

        // Named ">>" (Monad) or "*>" (Applicative) in Haskell parlance.
        MONAD<TResult> ReplaceBy<TResult>(MONAD<TResult> other);

        #endregion

        #region Basic Monad Functions

        #endregion
    }
}
