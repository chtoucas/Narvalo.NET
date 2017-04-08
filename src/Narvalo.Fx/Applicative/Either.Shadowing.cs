// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static partial class Either
    {
        internal static IEnumerable<TLeft> CollectAnyImpl<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> source)
        {
            Debug.Assert(source != null);

            foreach (var item in source)
            {
                if (item.IsLeft) { yield return item.Left; }
            }
        }
    }
}
