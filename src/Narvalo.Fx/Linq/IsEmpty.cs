// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
            => !@this.Any();
    }
}
