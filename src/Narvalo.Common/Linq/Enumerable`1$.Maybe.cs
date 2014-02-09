// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Fx;
    using Narvalo.Internal;

    public static partial class EnumerableExtensions
    {
        //// Restriction Operators

        public static IEnumerable<T> Where<T>(
            this IEnumerable<T> @this,
            Func<T, Maybe<bool>> predicateM)
        {
            Require.Object(@this);
            Require.NotNull(predicateM, "predicateM");

            return from item in @this
                   where predicateM.Invoke(item).Match(Stubs<bool>.Identity, defaultValue: false)
                   select item;
        }

        //// Conversion Operators

        public static IEnumerable<TResult> ConvertAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> converterM)
        {
            Require.Object(@this);
            Require.NotNull(converterM, "converterM");

            return from item in @this
                   let m = converterM.Invoke(item)
                   where m.IsSome
                   select m.Value;
        }

        //// Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return FirstOrNone(@this, Stubs<TSource>.True);
        }

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                return iter.MoveNext() ? iter.Current : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return LastOrNone(@this, Stubs<TSource>.True);
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    return Maybe<TSource>.None;
                }

                var value = iter.Current;
                while (iter.MoveNext()) {
                    value = iter.Current;
                }

                return value;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            return SingleOrNone(@this, Stubs<TSource>.True);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Maybe<TSource>.None;

                // On retourne Maybe.None si il y a encore un élément.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        //// Aggregate Operators

        public static Maybe<TSource> Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulator");

            using (var iter = @this.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                Maybe<TSource> result = Maybe.Create(iter.Current);

                while (iter.MoveNext()) {
                    result = result.Bind(_ => accumulatorM.Invoke(_, iter.Current));
                }

                return result;
            }
        }

        public static Maybe<TSource> ReduceBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, Maybe<TSource>> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Reduce(accumulatorM);
        }

        public static Maybe<TAccumulate> Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.Object(@this);
            Require.NotNull(accumulatorM, "accumulatorM");

            Maybe<TAccumulate> result = Maybe.Create(seed);

            foreach (TSource item in @this) {
                result = result.Bind(_ => accumulatorM.Invoke(_, item));
            }

            return result;
        }

        public static Maybe<TAccumulate> FoldBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, Maybe<TAccumulate>> accumulatorM)
        {
            Require.Object(@this);

            return @this.Reverse().Fold(seed, accumulatorM);
        }
    }
}
