// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad
    {
        public Func<T, Monad<TResult>> ComposeBack<T, TMiddle, TResult>(Func<TMiddle, Monad<TResult>> @this, Func<T, Monad<TMiddle>> thunk)
        {
            return _ => thunk.Invoke(_).Bind(@this);
        }
    }
}