// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;
    using System.Collections.Generic;

    // [Haskell] Control.Monad.MonadPlus
    // Monads that also support choice and failure.
    //
    // Translation map from Haskell to .NET:
    // - mzero          MonadPlus.Zero      <- Alternative::empty
    // - mplus          obj.Plus            <- Alternative::<|>
    //
    // Generalisations of list functions:
    // - msum           MonadPlus.Sum
    // - mfilter        obj.Where

    public interface IMonadPlus<T>
    {
        // [Haskell] mplus :: m a -> m a -> m a
        // An associative operation.
        MonadPlus<T> Plus(MonadPlus<T> other);

        // [Haskell] mfilter :: MonadPlus m => (a -> Bool) -> m a -> m a
        MonadPlus<T> Where(Func<T, bool> predicate);
    }

    public interface IMonadPlus
    {
        // [Haskell] mzero :: m a
        // The identity of mplus.
        MonadPlus<T> Zero<T>();

        // [Haskell] msum :: (Foldable t, MonadPlus m) => t (m a) -> m a
        // The sum of a collection of actions, generalizing concat.
        MonadPlus<TSource> Sum<TSource>(IEnumerable<MonadPlus<TSource>> @this);
    }

    public class MonadPlus<T>
    {

    }
}
