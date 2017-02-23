// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Fx;

    // [Haskell] Control.Monad.MonadPlus
    // Monads that also support choice and failure.
    //
    // API
    // ---
    // - mzero          MonadPlus.Zero      <- Alternative::empty
    // - mplus          obj.Plus            <- Alternative::<|>
    //
    // Generalisations of list functions:
    // - msum           MonadPlus.Sum
    // - mfilter        obj.Where
    //
    // Conditional execution of monadic expressions:
    // - guard          Operators.Guard
    //
    // Inherited:
    // - all methods from IMonad and IAlternative

    public interface IMonadPlus<T>
    {
        // [Haskell] mplus :: m a -> m a -> m a
        // An associative operation.
        Prototype<T> Plus(Prototype<T> other);

        // [Haskell] mzero :: m a
        // The identity of mplus.
        Prototype<T> Zero_();
    }

    public interface IMonadPlusSyntax<T>
    {
        // [Haskell] mfilter :: MonadPlus m => (a -> Bool) -> m a -> m a
        Prototype<T> Where(Func<T, bool> predicate);
    }

    public interface IMonadPlusOperators
    {
        // [Haskell] guard :: Alternative f => Bool -> f ()
        // guard b is pure () if b is True, and empty if b is False.
        Prototype<Unit> Guard(bool predicate);

        // [Haskell] msum :: (Foldable t, MonadPlus m) => t (m a) -> m a
        // The sum of a collection of actions, generalizing concat.
        Prototype<TSource> Sum<TSource>(IEnumerable<Prototype<TSource>> @this);
    }
}
