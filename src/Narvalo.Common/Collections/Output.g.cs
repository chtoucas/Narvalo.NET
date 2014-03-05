﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool. Changes to this file may cause incorrect
// behavior and will be lost if the code is regenerated.
//
// Runtime Version: 4.0.30319.34011
// </auto-generated>
//------------------------------------------------------------------------------

namespace Narvalo.Collections {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo;      // For Require
    using Narvalo.Fx;   // For Unit
    using Narvalo.Collections.Internal;

    /*!
     * Extensions for IEnumerable<Output<T>>.
     */
    public static partial class EnumerableOutputExtensions
    {
        #region Basic Monad functions (Prelude)

        /*!
         * Named `sequence` in Haskell parlance.
         */
        public static Output<IEnumerable<TSource>> Collect<TSource>(
            this IEnumerable<Output<TSource>> @this)
        {
            Require.Object(@this);

            return @this.CollectCore();
        }
        
        #endregion

    }

    // Extensions for IEnumerable<T>.
    public static partial class EnumerableExtensions
    {
        #region Basic Monad functions (Prelude)

        /// <remarks>
        /// Named <c>mapM</c> in Haskell parlance.
        /// </remarks>
        public static Output<IEnumerable<TResult>> Map<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.Object(@this);

            return @this.MapCore(funM);
        }
        
        #endregion

        #region Generalisations of list functions (Prelude)

        /// <remarks>
        /// Named <c>filterM</c> in Haskell parlance.
        /// </remarks>
        // REVIEW: Haskell use a differente signature.
        public static IEnumerable<TSource> Filter<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<bool>> predicateM)
        {
            Require.Object(@this);

            return @this.FilterCore(predicateM);
        }

        /// <remarks>
        /// Named <c>mapAndUnzipM</c> in Haskell parlance.
        /// </remarks>
        public static Output<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapAndUnzip<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<Tuple<TFirst, TSecond>>> funM)
        {
            Require.Object(@this);

            return @this.MapAndUnzipCore(funM);
        }

        /// <remarks>
        /// Named <c>zipWithM</c> in Haskell parlance.
        /// </remarks>
        public static Output<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Output<TResult>> resultSelectorM)
        {
            Require.Object(@this);

            return @this.ZipCore(second, resultSelectorM);
        }

        /// <remarks>
        /// Named <c>foldM</c> in Haskell parlance.
        /// </remarks>
        public static Output<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM)
        {
            Require.Object(@this);

            return @this.FoldCore(seed, accumulatorM);
        }

        #endregion
        
        #region Aggregate Operators

        public static Output<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM)
        {
             Require.Object(@this);

            return @this.FoldBackCore(seed, accumulatorM);
        }

        public static Output<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM)
        {
            Require.Object(@this);
            
            return @this.ReduceCore(accumulatorM);
        }

        public static Output<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM)
        {
            Require.Object(@this);

            return @this.ReduceBackCore(accumulatorM);
        }

        #endregion

        #region Catamorphisms

        // REVIEW: Signature.
        public static Output<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM,
            Func<Output<TAccumulate>, bool> predicate)
        {
            Require.Object(@this);

            return @this.FoldCore(seed, accumulatorM, predicate);
        }
        
        // REVIEW: Signature.
        public static Output<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM,
            Func<Output<TSource>, bool> predicate)
        {
            Require.Object(@this);

            return @this.ReduceCore(accumulatorM, predicate);
        }

        #endregion
    }
}

namespace Narvalo.Collections.Internal {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo;      // For Require
    using Narvalo.Fx;   // For Unit

    /*!
     * Internal extensions for IEnumerable<Output<T>>.
     */
    static partial class EnumerableOutputExtensions
    {
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(
            this IEnumerable<Output<TSource>> @this)
        {
            DebugCheck.NotNull(@this);

            var seed = Output.Success(Enumerable.Empty<TSource>());
            Func<Output<IEnumerable<TSource>>, Output<TSource>, Output<IEnumerable<TSource>>> fun
                = (m, n) =>
                    m.Bind(list =>
                    {
                        return n.Bind(item => Output.Success(
                            list.Concat(Enumerable.Repeat(item, 1))));
                    });

            return @this.Aggregate(seed, fun);
        }
    }

    // Internal extensions for IEnumerable<T>.
    static partial class EnumerableExtensions
    {
        internal static Output<IEnumerable<TResult>> MapCore<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            DebugCheck.NotNull(@this);

            return @this.Select(funM).Collect();
        }

        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<bool>> predicateM)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(predicateM, "predicateM");

            // NB: Haskell uses tail recursion, we don't.
            var list = new List<TSource>();

            foreach (var item in @this) {
                predicateM.Invoke(item)
                    .Run(_ =>
                    {
                        if (_ == true) {
                            list.Add(item);
                        }
                    });
            }

            return list;
        }

        internal static Output<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>>
            MapAndUnzipCore<TSource, TFirst, TSecond>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<Tuple<TFirst, TSecond>>> funM)
        {
            DebugCheck.NotNull(@this);

            return from tuple in @this.Select(funM).Collect()
                   let item1 = tuple.Select(_ => _.Item1)
                   let item2 = tuple.Select(_ => _.Item2)
                   select new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(item1, item2);
        }

        internal static Output<IEnumerable<TResult>> ZipCore<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Output<TResult>> resultSelectorM)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(resultSelectorM, "resultSelectorM");

            Func<TFirst, TSecond, Output<TResult>> resultSelector
                = (v1, v2) => resultSelectorM.Invoke(v1, v2);

            // WARNING: Do not remove resultSelector, otherwise .NET will make a recursive call
            // instead of using the Zip from Linq.
            return @this.Zip(second, resultSelector: resultSelector).Collect();
        }

        internal static Output<TAccumulate> FoldCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            Output<TAccumulate> result = Output.Success(seed);

            foreach (TSource item in @this) {
                result = result.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return result;
        }

        internal static Output<TAccumulate> FoldBackCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM)
        {
            DebugCheck.NotNull(@this);

            return @this.Reverse().Fold(seed, accumulatorM);
        }

        internal static Output<TSource> ReduceCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            using (var iter = @this.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Output<TSource> result = Output.Success(iter.Current);

                while (iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }

        internal static Output<TSource> ReduceBackCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM)
        {
            DebugCheck.NotNull(@this);

            return @this.Reverse().Reduce(accumulatorM);
        }

        internal static Output<TAccumulate> FoldCore<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Output<TAccumulate>> accumulatorM,
            Func<Output<TAccumulate>, bool> predicate)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(accumulatorM, "accumulatorM");
            Require.NotNull(predicate, "predicate");

            Output<TAccumulate> result = Output.Success(seed);

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }
            }

            return result;
        }

        internal static Output<TSource> ReduceCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Output<TSource>> accumulatorM,
            Func<Output<TSource>, bool> predicate)
        {
            DebugCheck.NotNull(@this);
            Require.NotNull(accumulatorM, "accumulatorM");
            Require.NotNull(predicate, "predicate");

            using (var iter = @this.GetEnumerator()) {if (!iter.MoveNext()) {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Output<TSource> result = Output.Success(iter.Current);

                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }
    }
}
