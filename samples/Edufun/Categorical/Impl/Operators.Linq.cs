// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Impl
{
    using System;
    using System.Collections.Generic;

    public class QueryOperators : IQueryOperators
    {
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
            throw new NotImplementedException();
        }

        public Monad<IEnumerable<TResult>> SelectWith<TSource, TResult>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Monad<IEnumerable<TSource>> WhereBy<TSource>(
            IEnumerable<TSource> source,
            Func<TSource, Monad<bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Monad<IEnumerable<TResult>> ZipWith<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, Monad<TResult>> resultSelector)
        {
            throw new NotImplementedException();
        }
    }
}