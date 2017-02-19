// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> SelectUnzip<TSource, TFirst, TSecond>(IEnumerable<TSource> @this, Func<TSource, Monad<Tuple<TFirst, TSecond>>> thunk)
        {
            throw new NotImplementedException();
        }
    }
}