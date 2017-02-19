// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Applicative<T>
    {
        // u *> v = pure (const id) <*> u <*> v
        // GHC.Base: a1 *> a2 = (id <$ a1) <*> a2
        public Applicative<TResult> ReplaceBy<TResult>(Applicative<TResult> other)
        {
            Func<TResult, TResult> id = _ => _;
            Func<T, Func<TResult, TResult>> value = _ => id;

            var applicative = Applicative.Of(value);
            Applicative<Func<TResult, TResult>> second = this.Gather(applicative);

            return other.Gather(second);
        }
    }
}
