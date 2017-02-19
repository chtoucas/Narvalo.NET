// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    public partial class Monad
    {
        Monad<T> IMonad.Join<T>(Monad<Monad<T>> square) => Join(square);
    }
}