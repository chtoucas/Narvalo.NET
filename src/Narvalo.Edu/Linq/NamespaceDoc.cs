// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System.Runtime.CompilerServices;

    /*!
     * The .NET Standard Query Operators
     * =================================
     * 
     * Linq                  | Monad<T>             | Monadic generalisation of list operations
     * ----------------------+----------------------+-------------------------------------------
     * ### Restriction Operators
     * ----------------------+----------------------+-------------------------------------------
     * Where (*)             | Monad<T>.Where       | @Enumerable<T>.Filter
     * ----------------------+----------------------+-------------------------------------------
     * ### Projection Operators
     * ----------------------+----------------------+-------------------------------------------
     * Select (*)            | Monad<T>.Select      | ??? @Enumerable<T>.ForEach
     * SelectMany (*)        | Monad<T>.Bind        | 
     *                       | @Monad<T>.SelectMany | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Partitioning Operators
     * ----------------------+----------------------+-------------------------------------------
     * Take                  | -                    |
     * Skip                  | -                    |
     * TakeWhile             | -                    |
     * SkipWhile             | -                    |
     * ----------------------+----------------------+-------------------------------------------
     * ### Join Operators
     * ----------------------+----------------------+-------------------------------------------
     * Join (*)              | @Monad<T>.Join       | 
     * GroupJoin (*)         | @Monad<T>.GroupJoin  | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Concatenation Operator
     * ----------------------+----------------------+-------------------------------------------
     * Concat                | @Monad<T>.Plus       | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Ordering Operators
     * ----------------------+----------------------+-------------------------------------------
     * OrderBy (*)           | ???                  | 
     * OrderByDescending (*) | ???                  |
     * ThenBy                | ???                  |
     * ThenByDescending      | ???                  |
     * Reverse               | -                    | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Grouping Operators
     * ----------------------+----------------------+-------------------------------------------
     * GroupBy (*)           | ???                  |
     * ----------------------+----------------------+-------------------------------------------
     * ### Set Operators
     * ----------------------+----------------------+-------------------------------------------
     * Distinct              | -                    |
     * Union                 | -                    |
     * Intersect             | -                    |
     * Except                | -                    |
     * ----------------------+----------------------+-------------------------------------------
     * ### Conversion Operators
     * ----------------------+----------------------+-------------------------------------------
     * AsEnumerable          | -                    |
     * ToArray               | -                    |
     * ToList                | -                    |
     * ToDictionary          | -                    |
     * ToLookup              | -                    |
     * OfType                | ???                  |
     * Cast                  | ???                  |
     * ----------------------+----------------------+-------------------------------------------
     * ### Equality Operator
     * ----------------------+----------------------+-------------------------------------------
     * SequenceEqual         | ??? Equals           |
     * ----------------------+----------------------+-------------------------------------------
     * ### Element Operators
     * ----------------------+----------------------+-------------------------------------------
     * First                 | -                    | @Enumerable<T>.FirstOrZero
     * FirstOrDefault        | -                    | @Enumerable<T>.FirstOrZero
     * Last                  | -                    | @Enumerable<T>.LastOrZero
     * LastOrDefault         | -                    | @Enumerable<T>.LastOrZero
     * Single                | -                    | @Enumerable<T>.SingleOrZero
     * SingleOrDefault       | -                    | @Enumerable<T>.SingleOrZero
     * ElementAt             | -                    |
     * ElementAtOrDefault    | -                    |
     * DefaultIfEmpty        | ??? Monad w/Zero     | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Generation Operators
     * ----------------------+----------------------+-------------------------------------------
     * Range                 | -                    |
     * Repeat                | @Monad<T>.Repeat     |
     * Empty                 | Monad<T>.Zero        | 
     * ----------------------+----------------------+-------------------------------------------
     * ### Quantifiers
     * ----------------------+----------------------+-------------------------------------------
     * Any                   | ???                  |
     * All                   | ???                  |
     * Contains              | ???                  |
     * ----------------------+----------------------+-------------------------------------------
     * ### Aggregate Operators
     * ----------------------+----------------------+-------------------------------------------
     * Count                 | -                    | 
     * LongCount             | -                    |
     * Sum                   | -                    | @Enumerable<Monad<T>>.Sum
     * Min                   | -                    |
     * Max                   | -                    |
     * Average               | -                    |
     * Aggregate             | -                    | @Enumerable<T>.Fold
     *                       | -                    | @Enumerable<T>.Reduce
     * ----------------------+----------------------+-------------------------------------------
     * 
     * 
     * Extensions to Linq
     * ==================
     * 
     * Linq                  | Monad<T>             | Monadic generalisation of list operations     
     * ----------------------+----------------------+-------------------------------------------
     * Append                |                      | -
     * Prepend               |                      | -
     * AggregateBack         |                      | @Enumerable<T>.FoldBack
     *                       |                      | @Enumerable<T>.Reduceback
     * Collect               |                      | @Enumerable<Monad<T>>.Collect
     * Zip                   |                      | @Enumerable<T>.Zip
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
