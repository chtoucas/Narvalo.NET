// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad
    {
        public Monad<TResult> Invoke<T, TResult>(Func<T, Monad<TResult>> @this, Monad<T> value)
        {
            throw new NotImplementedException();
        }
    }
}