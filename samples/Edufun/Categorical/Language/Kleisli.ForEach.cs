// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;

    public partial class Monad
    {
        public Monad<IEnumerable<TResult>> ForEach<T, TResult>(Func<T, Monad<TResult>> @this, IEnumerable<T> seq)
        {
            return SelectWith(seq, @this);
        }
    }
}