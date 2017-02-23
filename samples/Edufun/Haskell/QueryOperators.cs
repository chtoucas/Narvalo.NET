// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx.Linq;

    public class QueryOperators : IQueryOperators
    {
        private static readonly IMonadOperators s_Monad = new Monad();

        // [Control.Monad] foldM = foldlM
        public Monad<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
        {
            // [Haskell] foldlM :: (Foldable t, Monad m) => (b -> a -> m b) -> b -> t a -> m b
            // [Data.Foldable]
            //   foldlM f z0 xs = foldr f' return xs z0
            //     where f' x k z = f z x >>= k
            Func<Monad<TAccumulate>, TSource, Monad<TAccumulate>> acc
                = (acc1, arg2) => acc1.Bind(arg1 => accumulator(arg1, arg2));

            return source.Aggregate(Monad.Of(seed), acc);
        }

        // [Control.Monad] mapAndUnzipM f xs = unzip <$> traverse f xs
        public Monad<Tuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> SelectUnzip<TSource, TFirst, TSecond>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<Tuple<TFirst, TSecond>>> selector)
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
        public Monad<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<TResult>> selector)
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
        public Monad<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<bool>> predicate)
        {
            Func<bool, IEnumerable<TSource>, TSource, IEnumerable<TSource>> func
                = (flg, seq, item) => { if (flg) { return seq.Append(item); } else { return seq; } };

            var acc = s_Monad.Lift(func);

            Func<Monad<IEnumerable<TSource>>, TSource, Monad<IEnumerable<TSource>>> accumulator
                = (mseq, item) => acc(predicate(item), mseq, Monad.Of(item));

            return source.Aggregate(Monad.Of(Enumerable.Empty<TSource>()), accumulator);
        }

        // [Control.Monad] zipWithM f xs ys = sequenceA (zipWith f xs ys)
        public Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            Func<TFirst, TSecond, Monad<TResult>> selector
                = (arg1, arg2) => resultSelector.Invoke(arg1, arg2);

            IEnumerable<Monad<TResult>> seq = first.Zip(second, selector);

            return s_Monad.Collect(seq);
        }
    }
}