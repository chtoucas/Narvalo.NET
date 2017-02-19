// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Edufun.Categorical.Language;
    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    // [Haskell] Control.Applicative.Alternative
    // Amalgamation.
    //
    // Translation map from Haskell to .NET:
    // - empty      Alternative.Empty       (required)
    // - <|>        Alternative.Append      (required)
    // - some       Alternative.Some
    // - many       Alternative.Many
    //
    // Utility functions:
    // - optional   Alternative.Optional

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

    public class Alternative : IAlternative
    {
        // GHC.Base: empty = Nothing
        public Applicative<T> Empty<T>() { throw new FakeClassException(); }

        // GHC.Base:
        // Nothing <|> r = r
        // l <|> _ = l
        public Applicative<T> Append<T>(Applicative<T> first, Applicative<T> second)
        {
            throw new FakeClassException();
        }

        // some v = (:) <$> v <*> many v
        // GHC.Base:
        // some v = some_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Applicative<IEnumerable<T>> Some<T>(Applicative<T> value)
        {
            Func<T, Func<IEnumerable<T>, IEnumerable<T>>> append
                = _ => seq => Qperators.Append(seq, _);

            return Many(value).Gather(value.Select(append));
        }

        // many v = some v <|> pure []
        // GHC.Base:
        // many v = many_v
        //   where
        //     many_v = some_v <|> pure []
        //     some_v = (fmap(:) v) <*> many_v
        public Applicative<IEnumerable<T>> Many<T>(Applicative<T> value)
            => Append(Some(value), Applicative.Of(Enumerable.Empty<T>()));

        // optional v = Just <$> v <|> pure Nothing
        public Applicative<Maybe<T>> Optional<T>(Applicative<T> value)
            => Append(value.Select(Maybe.Of), Applicative.Of(Maybe<T>.None));
    }
}