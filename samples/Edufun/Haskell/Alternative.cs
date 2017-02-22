// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

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