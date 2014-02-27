// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Edu.Fx;

    public static class EnumerableMonadExtensions
    {
        #region Basic Monad functions

        // [Haskell] sequence
        public static Monad<IEnumerable<TSource>> Collect<TSource>(this IEnumerable<Monad<TSource>> @this)
        {
            Require.Object(@this);

            var seed = Monad.Return(Enumerable.Empty<TSource>());
            Func<Monad<IEnumerable<TSource>>, Monad<TSource>, Monad<IEnumerable<TSource>>> fun
                = (m, n) =>
                    m.Bind(list =>
                    {
                        return n.Bind(item => Monad.Return(list.Concat(Enumerable.Repeat(item, 1))));
                    });

            return @this.Aggregate(seed, fun);
        }

        #endregion

        #region Generalisations of list functions

#if !MONAD_DISABLE_ZERO && !MONAD_DISABLE_PLUS
        // [Haskell] msum
        public static Monad<TSource> Sum<TSource>(this IEnumerable<Monad<TSource>> @this)
        {
            Require.Object(@this);

            return @this.Aggregate(Monad<TSource>.Zero, (m, n) => m.Plus(n));
        }
#endif

        #endregion
    }
}
