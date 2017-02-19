// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Applicative
    {
        // GHC.Base: (<**>) = liftA2 (flip ($))
        public Applicative<TResult> Apply<T, TResult>(Applicative<Func<T, TResult>> @this, Applicative<T> value)
            => value.Gather(@this);
    }
}