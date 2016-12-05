// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using static System.Diagnostics.Contracts.Contract;

    public static class Sequence<TElement>
    {
        /// <summary>
        /// Gets the empty sequence that has the specified type argument.
        /// </summary>
        /// <remarks>
        /// Workaround for the fact that <see cref="Enumerable.Empty{TElement}"/> does not have any contract attached.
        /// </remarks>
        /// <value>An empty <see cref="IEnumerable{TElement}"/> whose type argument is TElement.</value>
        internal static IEnumerable<TElement> Empty
        {
            get
            {
                Ensures(Result<IEnumerable<TElement>>() != null);

                // We could use "yield break", but Enumerable.Empty<T> is more readable
                // with the additional benefit of returning a singleton.
                var coll = Enumerable.Empty<TElement>();
                Contract.Assume(coll != null);

                return coll;
            }
        }
    }
}
