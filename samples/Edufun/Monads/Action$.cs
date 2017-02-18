// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Monads
{
    using System;

    using Narvalo.Fx;

    public static class ActionExtensions
    {
        public static Kunc<Unit, Unit> ToKunc(this Action @this)
            => _ => { @this.Invoke(); return Monad.Unit; };

        public static Kunc<TSource, Unit> ToKunc<TSource>(this Action<TSource> @this)
            => _ => { @this.Invoke(_); return Monad.Unit; };
    }
}
