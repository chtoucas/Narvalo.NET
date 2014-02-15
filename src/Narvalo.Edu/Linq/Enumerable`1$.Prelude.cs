// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Narvalo.Edu.Fx;

    static partial class EnumerableExtensions
    {
        #region Basic Monad functions

        // [Haskell] forM
        public static Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Kunc<TSource, TResult> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return (from _ in @this select kun.Invoke(_)).Collect();
        }

        #endregion

        #region Generalisations of list functions

        // [Haskell] filterM
        public static Monad<IEnumerable<TSource>> Where<TSource>(
            this IEnumerable<TSource> @this,
            Kunc<TSource, bool> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            throw new NotImplementedException();

            //var list = from item in @this
            //           let b = predicateM(item)
            //           select item;

            //var list = from item in @this
            //           where predicateM(item).Match(b => b, false)
            //           select item;

            //return Monad.Return(list);
        }

        // [Haskell] mapAndUnzipM
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Optional extension.")]
        static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> SelectAndUnzip<TSource, TFirst, TSecond>(
           this IEnumerable<TSource> @this,
           Kunc<TSource, Tuple<TFirst, TSecond>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return from _ in
                       (from _ in @this select kun(_)).Collect()
                   let item1 = from item in _ select item.Item1
                   let item2 = from item in _ select item.Item2
                   select new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(item1, item2);
        }

        // [Haskell] zipWithM
        public static Monad<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> @this,
            IEnumerable<TSecond> second,
            Kunc<TFirst, TSecond, TResult> resultSelectorM)
        {
            Require.Object(@this);
            Require.NotNull(second, "second");
            Require.NotNull(resultSelectorM, "resultSelectorM");

            Func<TFirst, TSecond, Monad<TResult>> resultSelector = (v1, v2) => resultSelectorM.Invoke(v1, v2);

            return (@this.Zip(second, resultSelector)).Collect();
        }

        // [Haskell] foldM
        public static Monad<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Kunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            Monad<TAccumulate> result = Monad.Return(seed);

            foreach (TSource item in @this) {
                result = result.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return result;
        }

        #endregion
    }
}
