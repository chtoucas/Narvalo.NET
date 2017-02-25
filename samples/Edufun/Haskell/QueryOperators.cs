// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

//#define STRICT_HASKELL

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;
    using Narvalo.Fx.Linq;

    public class QueryOperators : IQueryOperators
    {
        private static readonly IMonadOperators s_Monad = new Prototype();

        // [Control.Monad] foldM = foldlM
        public Prototype<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Prototype<TAccumulate>> accumulator)
        {
            // [Haskell] foldlM :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m b
            // [Data.Foldable]
            //   foldlM f z0 xs = foldr f' return xs z0
            //     where f' x k z = f z x >>= k
            Func<Prototype<TAccumulate>, TSource, Prototype<TAccumulate>> func
                = (acc, arg2) => acc.Bind(arg1 => accumulator(arg1, arg2));

            return source.Aggregate(Prototype.Of(seed), func);
        }

        // [Control.Monad] mapAndUnzipM f xs = unzip <$> traverse f xs
        public Prototype<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> SelectUnzip<TSource, TFirst, TSecond>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<Tuple<TFirst, TSecond>>> selector)
        {
            return SelectWith(source, selector).Select(
                tuples =>
                {
                    IEnumerable<TFirst> list1 = tuples.Select(_ => _.Item1);
                    IEnumerable<TSecond> list2 = tuples.Select(_ => _.Item2);

                    return new Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>(list1, list2);
                });
        }

        // [Data.Traversable] mapM = traverse
        public Prototype<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<TResult>> selector)
        {
            // The signature of mapM is
            //   mapM :: (Traversable t, Monad m) => (a -> m b) -> t a -> m (t b)
            // Here we interpret Traversable as IEnumerable.
            //   [Haskell] traverse :: Applicative f => (a -> f b) -> t a -> f (t b)
            //   [Data.Traversable] traverse f = sequenceA . fmap f
            // Here we interpret f as Monad<T>.
            // where sequenceA is in fact Monad::sequence (see Monad<T>.Collect()).
            return s_Monad.Collect(source.Select(selector));
        }

        // [Control.Monad] filterM p = foldr (\ x -> liftA2 (\ flg -> if flg then (x:) else id) (p x)) (pure [])
        public Prototype<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<bool>> predicate)
        {
#if STRICT_HASKELL
            Func<TSource, Func<bool, IEnumerable<TSource>, IEnumerable<TSource>>> func
                = item => (b, seq) => b ? seq.Append(item) : seq;

            Func<Prototype<IEnumerable<TSource>>, TSource, Prototype<IEnumerable<TSource>>> accumulator
                = (mseq, item) => predicate(item).Zip(mseq, func(item));

            return source.Aggregate(Prototype.Of(Enumerable.Empty<TSource>()), accumulator);
#else
            return Prototype.Of(WhereByIterator(source, predicate));
#endif
        }

        public static IEnumerable<TSource> WhereByIterator<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Prototype<bool>> predicate)
        {
            using (var iter = source.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    bool pass = false;
                    TSource item = iter.Current;

                    predicate(item).Bind(val =>
                    {
                        pass = val;

                        return Prototype.Of(Unit.Single);
                    });

                    if (pass) { yield return item; }
                }
            }
        }

        // [Control.Monad] zipWithM f xs ys = sequenceA (zipWith f xs ys)
        public Prototype<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Prototype<TResult>> resultSelector)
        {
            IEnumerable<Prototype<TResult>> seq = first.Zip(second, resultSelector);

            return s_Monad.Collect(seq);
        }
    }
}