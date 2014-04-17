// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo;
    using Narvalo.Collections;

    internal static class WeakEquality
    {
        /// <summary>
        /// An order independent version of <see cref="Enumerable.SequenceEqual{TSource}(System.Collections.Generic.IEnumerable{TSource},System.Collections.Generic.IEnumerable{TSource})"/>.
        /// </summary>
        public static bool AreEqual<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            Require.NotNull(left, "left");
            Require.NotNull(right, "right");

            var leftObjects = left.ToList();
            var rightObjects = right.ToList();

            if (leftObjects.Count != rightObjects.Count) {
                return false;
            }

            foreach (var item in rightObjects) {
                if (!leftObjects.Contains(item)) {
                    return false;
                }

                leftObjects.Remove(item);
            }

            return leftObjects.IsEmpty();
        }
    }
}
