// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<TAccumulate> Fold<TSource, TAccumulate>(IEnumerable<TSource> @this, TAccumulate seed, Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
        {
            throw new NotImplementedException();
        }
    }
}