// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Linq
{
    /// <summary>
    /// Provides a set of static and extension methods for querying objects
    /// that implement <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    /// <remarks>
    /// New LINQ operators:
    /// - Projecting: SelectAny
    /// - Filtering: WhereAny
    /// - Set: Append, Prepend
    /// - Element: FirstOrNone, LastOrNone, SingleOrNone
    /// - Aggregation (catamorphisms): Aggregate
    /// - Quantifiers: IsEmpty
    /// - Generation: EmptyIfNull
    /// We have also operators accepting arguments in the Kleisli "category":
    /// SelectWith, SelectUnzip, ZipWith, WhereBy, Fold, Reduce.
    /// </remarks>
    /// For more generation operators, <seealso cref="Sequence"/>.
    public static partial class Qperators { }
}
