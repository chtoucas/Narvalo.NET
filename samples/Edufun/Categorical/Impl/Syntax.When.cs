// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Impl
{
    using System;
    using System.Collections.Generic;

    public partial class Monad<T>
    {
        public void When(Func<T, bool> predicate, Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}