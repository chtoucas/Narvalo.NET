// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(IEnumerable<TFirst> @this, IEnumerable<TSecond> second, Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            throw new NotImplementedException();
        }
    }
}