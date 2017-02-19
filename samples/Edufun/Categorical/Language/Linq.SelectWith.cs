// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<IEnumerable<TResult>> SelectWith<T, TResult>(IEnumerable<T> @this, Func<T, Monad<TResult>> selector)
        {
            throw new NotImplementedException();
        }
    }
}
