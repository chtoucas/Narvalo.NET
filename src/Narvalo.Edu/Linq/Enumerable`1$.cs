// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Edu.Fx;

    static partial class EnumerableExtensions
    {
        #region Aggregate Operators

        public static Monad<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Kunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Fold(seed, accumulatorM);
        }

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

                // On retourne Monad.Zero si il y a encore un élément.
                return iter.MoveNext() ? Monad<TSource>.Zero : result;
            }
        }
#endif

        #endregion
    }
}
