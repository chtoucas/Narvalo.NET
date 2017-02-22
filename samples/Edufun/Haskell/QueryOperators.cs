// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Haskell
{
    using System;
    using System.Collections.Generic;

    using System.Linq;

    public class QueryOperators : IQueryOperators
    {
        private static readonly IMonadOperators s_Monad = new Monad();

        public Monad<TAccumulate> Fold<TSource, TAccumulate>(
            IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, Monad<TAccumulate>> accumulator)
        {
            throw new NotImplementedException();
        }

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

        public Monad<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<TResult>> selector)
        {
            throw new NotImplementedException();
        }

        // [Control.Monad] filterM p = foldr (\ x -> liftA2 (\ flg -> if flg then (x:) else id) (p x)) (pure [])
        public Monad<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<bool>> predicate)
        {
            throw new NotImplementedException();
        }

        // [Control.Monad] zipWithM f xs ys = sequenceA (zipWith f xs ys)
        public Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            Func<TFirst, TSecond, Monad<TResult>> selector
                = (v1, v2) => resultSelector.Invoke(v1, v2);

            IEnumerable<Monad<TResult>> seq = first.Zip(second, selector);

            return s_Monad.Collect(seq);
        }
    }
}