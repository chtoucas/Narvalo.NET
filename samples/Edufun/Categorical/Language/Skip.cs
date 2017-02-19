// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using Narvalo.Fx;

    public partial class Functor<T>
    {
        // void x = () <$ x
        public Functor<Unit> Skip() => Replace(Unit.Single);
    }
}
