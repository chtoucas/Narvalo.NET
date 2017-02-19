// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<IEnumerable<TSource>> WhereBy<TSource>(IEnumerable<TSource> @this, Func<TSource, Monad<bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}