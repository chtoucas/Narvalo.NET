// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Narvalo.Fx;

    /// <summary>
    /// Provides extension methods for <see cref="System.Collections.Generic.IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
        {
            Require.Object(@this);

            return !@this.Any();
        }

        #region Concatenation Operators

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.Object(@this);

            return @this.Concat(Enumerable.Repeat(element, 1));
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            return Enumerable.Repeat(element, 1).Concat(@this);
        }

        #endregion

        #region Conversion Operators

        public static ICollection<TSource> ToCollection<TSource>(this IEnumerable<TSource> @this)
        {
            Require.Object(@this);

            var result = new Collection<TSource>();

            foreach (TSource item in @this) {
                result.Add(item);
            }

            return result;
        }

        #endregion

        #region Aggregate Operators

        public static TAccumulate AggregateBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Require.Object(@this);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static TSource AggregateBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator)
        {
            Require.Object(@this);

            return @this.Reverse().Aggregate(accumulator);
        }

        #endregion

        #region Catamorphisms

        public static TAccumulate Fold<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator,
            Func<TAccumulate, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(accumulator, "accumulator");
            Require.NotNull(predicate, "predicate");

            TAccumulate result = seed;

            using (var iter = @this.GetEnumerator()) {
                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = accumulator.Invoke(result, iter.Current);
                }
            }

            return result;
        }

        public static TSource Reduce<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(accumulator, "accumulator");
            Require.NotNull(predicate, "predicate");

            using (var iter = @this.GetEnumerator()) {
                if (!iter.MoveNext()) {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                TSource result = iter.Current;

                while (predicate.Invoke(result) && iter.MoveNext()) {
                    result = accumulator.Invoke(result, iter.Current);
                }

                return result;
            }
        }

        #endregion
    }

    // Extensions using monadic classes.
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            return from _ in @this
                   let m = funM.Invoke(_)
                   where m.IsSome
                   select m.Value;
        }

        public static IEnumerable<TResult> MapAny<TSource, TResult>(
            this IEnumerable<TSource> @this,
            Func<TSource, Output<TResult>> funM)
        {
            Require.Object(@this);
            Require.NotNull(funM, "funM");

            return from _ in @this
                   let m = funM.Invoke(_)
                   where m.IsSuccess
                   select m.Value;
        }

        #region Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);

            return FirstOrNone(@this, _ => true);
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
            Contract.Requires(@this != null);

            return LastOrNone(@this, _ => true);
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
            Contract.Requires(@this != null);

            return SingleOrNone(@this, _ => true);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            var seq = from t in @this where predicate.Invoke(t) select Maybe.Create(t);
            using (var iter = seq.GetEnumerator()) {
                var result = iter.MoveNext() ? iter.Current : Maybe<TSource>.None;

                // Return Maybe.None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion
    }

    // Optimized extensions for Maybe<T>.
    public static partial class EnumerableExtensions
    {
        internal static IEnumerable<TSource> FilterCore<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, Maybe<bool>> predicateM)
        {
            Require.NotNull(predicateM, "predicateM");
            DebugCheck.NotNull(@this);

            return from _ in @this
                   where predicateM.Invoke(_).ValueOrElse(false)
                   select _;
        }
    }
}
