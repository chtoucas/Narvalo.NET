// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerator{T}"/>.
    /// </summary>
    public static partial class EnumeratorExtensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerator<T> @this)
        {
            Require.NotNull(@this, nameof(@this));

            var coll = new Collection<T>();

            while (@this.MoveNext()) {
                coll.Add(@this.Current);
            }

            return coll;
        }
    }
}
