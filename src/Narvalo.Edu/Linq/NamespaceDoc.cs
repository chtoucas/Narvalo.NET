// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System.Runtime.CompilerServices;

    /*!
     * The .NET Standard Query Operators
     * =================================
     * 
     * ### Restriction Operators
     * - Where (*)                      Monad<T>.Where
     * ### Projection Operators
     * - Select (*)                     Monad<T>.Select
     * - SelectMany (*)                 Monad<T>.Bind
     *                                  @Monad<T>.SelectMany
     * ### Partitioning Operators
     * - Take                           -
     * - Skip                           -
     * - TakeWhile                      -
     * - SkipWhile                      -
     * ### Join Operators
     * - Join (*)                       @Monad<T>.Join
     * - GroupJoin (*)                  @Monad<T>.GroupJoin
     * ### Concatenation Operator
     * - Concat                         ??? @Monad<T>.Plus
     * ### Ordering Operators
     * - OrderBy (*)                    @Monad<T>.OrderBy
     * - OrderByDescending (*)          @Monad<T>.OrderByDescending
     * - ThenBy                         ???
     * - ThenByDescending               ???
     * - Reverse                        -
     * ### Grouping Operators
     * - GroupBy (*)                    @Monad<T>.GroupBy
     * ### Set Operators
     * - Distinct                       -
     * - Union                          -
     * - Intersect                      -
     * - Except                         -
     * ### Conversion Operators
     * - AsEnumerable                   -
     * - ToArray                        -
     * - ToList                         -
     * - ToDictionary                   -
     * - ToLookup                       -
     * - OfType                         ???
     * - Cast                           ???
     * ### Equality Operator
     * - SequenceEqual                  ??? Equals
     * ### Element Operators
     * - First                          -
     * - FirstOrDefault                 -
     * - Last                           -
     * - LastOrDefault                  -
     * - Single                         -
     * - SingleOrDefault                -
     * - ElementAt                      -
     * - ElementAtOrDefault             -
     * - DefaultIfEmpty                 ??? For Monad with a Zero
     * ### Generation Operators
     * - Range                          -
     * - Repeat                         -
     * - Empty                          Monad<T>.Zero
     * ### Quantifiers
     * - Any                            ???
     * - All                            ???
     * - Contains                       ???
     * ### Aggregate Operators
     * - Count                          -
     * - LongCount                      -
     * - Sum                            -
     * - Min                            -
     * - Max                            -
     * - Average                        -
     * - Aggregate                      -
     * 
     * 
     * Monadic extensions to List operations
     * =====================================
     * 
     * Linq            | List           | Terminology used here
     * ----------------+----------------+-----------------------------------------
     * Aggregate       | FoldLeft       | Fold or Reduce (if no initial value)
     *                 | FoldRight      | FoldBack or ReduceBack (if no initial value)
     *                 | Cons           | Prepend
     *                 | Scons          | Append
     * 
     * 
     * References
     * ----------
     * 
     * + Query Expression Pattern / C# 5.0 Language Specification. Section 7.16.3
     *   http://www.microsoft.com/en-us/download/details.aspx?id=7029
     * + The .NET Standard Query Operators
     *   http://msdn.microsoft.com/en-us/library/bb394939.aspx
     * + Classification of Standard Query Operators by Manner of Execution
     *   http://msdn.microsoft.com/en-us/library/bb882641.aspx
     * + http://en.wikipedia.org/wiki/Language_Integrated_Query
     */

    [CompilerGeneratedAttribute]
    class NamespaceDoc
    {
    }
}
