// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Qperators
    {
        /// <summary>
        /// Returns true if the sequence is empty; otherwise false.
        /// </summary>
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
            => !@this.Any();
    }
}
