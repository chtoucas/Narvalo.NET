// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Language
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx.Linq;

    public partial class Monad
    {
        public Monad<IEnumerable<T>> Collect<T>(IEnumerable<Monad<T>> @this)
        {
            var seed = Monad.Of(Enumerable.Empty<T>());
            Func<IEnumerable<T>, T, IEnumerable<T>> append = (m, item) => m.Append(item);

            return @this.Aggregate(seed, Lift(append));
        }
    }
}