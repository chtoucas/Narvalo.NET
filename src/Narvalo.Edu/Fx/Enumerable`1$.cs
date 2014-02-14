// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class EnumerableExtensions
    {
        //// Restriction Operators

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

        //// Conversion Operators

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

        //// Element Operators

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

        //// Aggregate Operators

        public static Monad<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Kunc<TSource, TSource, TSource> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulator");

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

        public static Monad<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Kunc<TAccumulate, TSource, TAccumulate> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Fold(seed, accumulatorM);
        }
    }
}
