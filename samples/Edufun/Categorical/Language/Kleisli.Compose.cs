// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad
    {
        public Func<T, Monad<TResult>> Compose<T, TMiddle, TResult>(Func<T, Monad<TMiddle>> @this, Func<TMiddle, Monad<TResult>> thunk)
        {
            return _ => @this.Invoke(_).Bind(thunk);
        }
    }
}