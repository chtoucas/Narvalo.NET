// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;

    public partial class Monad<T>
    {
        Monad<TResult> IMonad<T>.Bind<TResult>(Func<T, Monad<TResult>> selector)
            => Bind(selector);
    }
}