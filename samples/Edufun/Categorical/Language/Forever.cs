// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad<T>
    {
        public Monad<TResult> Forever<TSource, TResult>(Func<Monad<TResult>> thunk)
        {
            throw new NotImplementedException();
        }
    }
}