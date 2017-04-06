// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static and extension methods for querying or producing
    /// objects that implement <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <remarks>
    /// New LINQ operators:
    /// - Projecting: SelectAny (deferred)
    /// - Filtering: WhereAny (deferred)
    /// - Set: Append (deferred), Prepend (deferred)
    /// - Element: FirstOrNone, LastOrNone, SingleOrNone, ElementAtOrNone
    /// - Aggregation: Aggregate (deferred)
    /// - Quantifiers: IsEmpty
    /// - Generation: EmptyIfNull
    /// We have also operators accepting arguments in the Kleisli "category":
    /// SelectWith (deferred), ZipWith (deferred), WhereBy (deferred), Fold, Reduce.
    /// </remarks>
    public static partial class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Of<TSource>(TSource value)
        {
            // Enumerable.Repeat(value, 1) works too, but is less readable.
            yield return value;
        }
    }
}
