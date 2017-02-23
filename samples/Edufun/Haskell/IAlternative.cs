// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System.Collections.Generic;

    using Narvalo.Fx;

    // [Haskell] Control.Applicative.Alternative
    // Amalgamation.
    //
    // API
    // ---
    // - empty      Alternative.Empty       (required)
    // - <|>        Alternative.Append      (required)
    // - some       Alternative.Some
    // - many       Alternative.Many
    //
    // Utility functions:
    // - optional   Alternative.Optional
    //
    // Inherited:
    // - all methods from IApplicative

    public interface IAlternative
    {
        // [Haskell] empty :: f a
        // The identity of <|>.
        Applicative<T> Empty<T>();

        // [Haskell] (<|>) :: f a -> f a -> f a
        // An associative binary operation.
        Applicative<T> Append<T>(Applicative<T> first, Applicative<T> second);

        // [Haskell] some :: f a -> f [a]
        // One or more.
        Applicative<IEnumerable<T>> Some<T>(Applicative<T> value);

        // [Haskell] many :: f a -> f [a]
        // Zero or more.
        Applicative<IEnumerable<T>> Many<T>(Applicative<T> value);

        // [Haskell] optional :: Alternative f => f a -> f (Maybe a)
        // One or none.
        Applicative<Maybe<T>> Optional<T>(Applicative<T> value);
    }
}