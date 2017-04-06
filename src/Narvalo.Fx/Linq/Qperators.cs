// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    /// <summary>
    /// Provides a set of static and extension methods for querying
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
    public static partial class Qperators { }
}
