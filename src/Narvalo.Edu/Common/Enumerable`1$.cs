// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Append<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return AppendCore_(@this, element);
        }

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> @this, T element)
        {
            Require.Object(@this);

            return PrependCore_(@this, element);
        }

        static IEnumerable<T> AppendCore_<T>(IEnumerable<T> source, T element)
        {
            foreach (var item in source) {
                yield return item;
            }

            yield return element;
        }

        static IEnumerable<T> PrependCore_<T>(IEnumerable<T> source, T element)
        {
            yield return element;

            foreach (var item in source) {
                yield return item;
            }
        }
    }
}
