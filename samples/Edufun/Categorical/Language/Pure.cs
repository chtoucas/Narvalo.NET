// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    public partial class Applicative
    {
        Applicative<T> IApplicative.Pure<T>(T value) => Pure(value);
    }

    public partial class Monad
    {
        Monad<T> IMonad.Pure<T>(T value) => Pure(value);
    }
}