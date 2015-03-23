// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Narvalo.Internal;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);

            return !@this.Any();
        }

        #region Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return FirstOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Of(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                return iter.MoveNext() ? iter.Current.AssumeNotNull() : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return LastOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Of(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    return Maybe<TSource>.None;
                }

                var value = iter.Current.AssumeNotNull();
                while (iter.MoveNext())
                {
                    value = iter.Current.AssumeNotNull();
                }

                return value;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            return SingleOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<Maybe<TSource>>() != null);

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Of(t);

            using (var iter = seq.AssumeNotNull().GetEnumerator())
            {
                var result = iter.MoveNext() ? iter.Current.AssumeNotNull() : Maybe<TSource>.None;

                // Return Maybe.None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion

        #region Concatenation Operators

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return @this.Concat(Sequence.Single(element)).AssumeNotNull();
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Contract.Requires(@this != null);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return Sequence.Single(element).Concat(@this).AssumeNotNull();
        }

        #endregion

        #region Conversion Operators

        public static ICollection<TSource> ToCollection<TSource>(this IEnumerable<TSource> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<ICollection<TSource>>() != null);

            var result = new Collection<TSource>();

            foreach (TSource item in @this)
            {
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
            Contract.Requires(@this != null);
            Contract.Requires(accumulator != null);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static TSource AggregateBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator)
        {
            Contract.Requires(@this != null);
            Contract.Requires(accumulator != null);

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

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(result) && iter.MoveNext())
                {
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

            using (var iter = @this.GetEnumerator())
            {
                if (!iter.MoveNext())
                {
                    throw new InvalidOperationException("Source sequence was empty.");
                }

                TSource result = iter.Current;

                while (predicate.Invoke(result) && iter.MoveNext())
                {
                    result = accumulator.Invoke(result, iter.Current);
                }

                return result;
            }
        }

        #endregion
    }
}
