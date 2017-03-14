// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun
{
    using System.Collections.Generic;

    using Narvalo.Applicative;

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

    public interface IAlternative<T>
    {
        // [Haskell] (<|>) :: f a -> f a -> f a
        // An associative binary operation.
        Prototype<T> Append(Prototype<T> value);

        // [Haskell] empty :: f a
        // The identity of <|>.
        Prototype<T> Empty_();
    }

    public interface IAlternativeOperators
    {
        // [Haskell] many :: f a -> f [a]
        // Zero or more.
        // Guess: Try until it fails.
        Prototype<IEnumerable<T>> Many<T>(Prototype<T> value);

        // [Haskell] optional :: Alternative f => f a -> f (Maybe a)
        // One or none.
        Prototype<Maybe<T>> Optional<T>(Prototype<T> value);

        // [Haskell] some :: f a -> f [a]
        // One or more.
        // Guess: Try until it succeeds, and continue until it fails.
        Prototype<IEnumerable<T>> Some<T>(Prototype<T> value);
    }
}