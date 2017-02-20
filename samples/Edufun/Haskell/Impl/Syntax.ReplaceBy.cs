// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell.Impl
{
    using System;

    public partial class Applicative<T>
    {
        // [GHC.Base] a1 *> a2 = (id <$ a1) <*> a2
        // [Control.Applicative] u *> v = pure (const id) <*> u <*> v
        public Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other)
        {
            Func<TResult, TResult> id = _ => _;
            Func<T, Func<TResult, TResult>> value = _ => id;

            var applicative = Applicative.Of(value);
            Applicative<Func<TResult, TResult>> second = this.Gather(applicative);

            return other.Gather(second);
        }
    }

    public partial class Monad<T>
    {
        public Monad<TResult> ReplaceBy<TResult>(Monad<TResult> other)
            => Bind(_ => other);
    }
}
