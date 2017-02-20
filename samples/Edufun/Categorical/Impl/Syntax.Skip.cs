// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Impl
{
    using Narvalo.Fx;

    public partial class Functor<T>
    {
        // [Control.Functor] void x = () <$ x
        public Functor<Unit> Skip() => Replace(Unit.Single);
    }

    public partial class Monad<T>
    {
        // See Functor<T>.Skip().
        public Monad<Unit> Skip() => Monad.Unit;
    }
}
