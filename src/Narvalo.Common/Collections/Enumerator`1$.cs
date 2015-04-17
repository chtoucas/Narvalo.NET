// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerator{T}"/>.
    /// </summary>
    public static partial class EnumeratorExtensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerator<T> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Collection<T>>() != null);

            var retval = new Collection<T>();

            while (@this.MoveNext()) {
                retval.Add(@this.Current);
            }

            return retval;
        }
    }
}
