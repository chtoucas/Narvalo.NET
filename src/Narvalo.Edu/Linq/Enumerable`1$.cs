// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Narvalo.Edu.Fx;
    using Narvalo.Linq;

    static partial class EnumerableExtensions
    {
        #region Basic Monad functions

        // [Haskell] forM
        public static Monad<IEnumerable<TResult>> ForEach<TSource, TResult>(
            IEnumerable<TSource> @this,
            Kunc<TSource, TResult> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return Collect(from _ in @this select kun.Invoke(_));
        }

        // [Haskell] sequence
        public static Monad<IEnumerable<TSource>> Collect<TSource>(IEnumerable<Monad<TSource>> @this)
        {
            Require.Object(@this);

            var seed = Monad.Return(Enumerable.Empty<TSource>());
            Func<Monad<IEnumerable<TSource>>, Monad<TSource>, Monad<IEnumerable<TSource>>> func
                = (m, n) =>
                    m.Bind(list =>
                    {
                        return n.Bind(item => Monad.Return(list.Append(item)));
                    });

            return @this.Aggregate(seed, func);
        }

        #endregion

        #region Generalisations of list functions

#if !MONAD_DISABLE_ZERO && !MONAD_DISABLE_PLUS
        // [Haskell] msum
        public static Monad<TSource> Sum<TSource>(IEnumerable<Monad<TSource>> @this)
        {
            Require.Object(@this);

            return @this.Aggregate(Monad<TSource>.Zero, (m, n) => m.Plus(n));
        }
#endif

        // [Haskell] filterM
        public static Monad<IEnumerable<TSource>> Where<TSource>(
            IEnumerable<TSource> @this,
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
        static Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> MapAndUnzip<TSource, TFirst, TSecond>(
           IEnumerable<TSource> @this,
           Kunc<TSource, Tuple<TFirst, TSecond>> kun)
        {
            Require.Object(@this);
            Require.NotNull(kun, "kun");

            return from _ in
                       Collect(from _ in @this select kun(_))
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

            return Collect(@this.Zip(second, resultSelector));
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

        #region Restriction Operators

        //public static IEnumerable<T> Where<T>(
        //    this IEnumerable<T> @this,
        //    Func<T, Monad<bool>> predicateM)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(predicateM, "predicateM");

        //    return from item in @this
        //           where predicateM.Invoke(item).ValueOrElse(false)
        //           select item;
        //}

        #endregion

        #region Aggregate Operators

        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Kunc<TSource, TSource, TSource> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            using (var iter = @this.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Monad<TSource> result = Monad.Return(iter.Current);

                while (iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }

        public static Monad<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Kunc<TSource, TSource, TSource> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Reduce(accumulatorM);
        }

        #endregion

        #region Conversion Operators

        //public static IEnumerable<TResult> ConvertAny<TSource, TResult>(
        //    this IEnumerable<TSource> @this,
        //    Func<TSource, Monad<TResult>> converterM)
        //{
        //    Require.Object(@this);
        //    Require.NotNull(converterM, "converterM");

        //    return from item in @this
        //           let m = converterM.Invoke(item)
        //           where m.IsSome
        //           select m.Value;
        //}

        #endregion

        public static Monad<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Kunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Fold(seed, accumulatorM);
        }

        #region Element Operators

#if !MONAD_DISABLE_ZERO
        public static Monad<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return FirstOrNone(@this, _ => true);
        }

        public static Monad<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Monad.Return(t);
            using (var iter = seq.GetEnumerator()) {
                return iter.MoveNext() ? iter.Current : Monad<TSource>.Zero;
            }
        }

        public static Monad<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return LastOrNone(@this, _ => true);
        }

        public static Monad<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Monad.Return(t);
            using (var iter = seq.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    return Monad<TSource>.Zero;
                }

                var value = iter.Current;
                while (iter.MoveNext()) {
                    value = iter.Current;
                }

                return value;
            }
        }

        public static Monad<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return SingleOrNone(@this, _ => true);
        }

        public static Monad<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Monad.Return(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Monad<TSource>.Zero;

                // On retourne Monad.None si il y a encore un élément.
                return iter.MoveNext() ? Monad<TSource>.Zero : result;
            }
        }
#endif

        #endregion
    }
}
