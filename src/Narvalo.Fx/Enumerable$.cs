// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class EnumerableExtensions
    {
        // Useful when using built-in LINQ operators. Even if it is not publicly visible,
        // I think that all LINQ operators never return a null but rather an empty sequence if needed.
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> @this)
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            if (@this == null)
            {
                return Sequence.Empty<TSource>();
            }

            return @this;
        }

        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
        {
            Acknowledge.Object(@this);

            return !@this.Any();
        }

        #region Element Operators

        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Acknowledge.Object(@this);

            return FirstOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> FirstOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Of(t);

            using (var iter = seq.EmptyIfNull().GetEnumerator())
            {
                var current = iter.Current;

                return iter.MoveNext() ? current : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Acknowledge.Object(@this);

            return LastOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            return @this.Reverse().EmptyIfNull().FirstOrNone();
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Acknowledge.Object(@this);

            return SingleOrNone(@this, Stubs<TSource>.AlwaysTrue);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.Object(@this);
            Require.NotNull(predicate, "predicate");

            IEnumerable<Maybe<TSource>> seq
                = from t in @this where predicate.Invoke(t) select Maybe.Of(t);

            using (var iter = seq.EmptyIfNull().GetEnumerator())
            {
                var current = iter.Current;

                var result = iter.MoveNext() ? current : Maybe<TSource>.None;

                // Return Maybe.None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion

        #region Concatenation Operators

        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Acknowledge.Object(@this);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return @this.Concat(Sequence.Single(element)).EmptyIfNull();
        }

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Acknowledge.Object(@this);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return Sequence.Single(element).Concat(@this).EmptyIfNull();
        }

        #endregion

        #region Conversion Operators

        public static ICollection<TSource> ToCollection<TSource>(this IEnumerable<TSource> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<ICollection<TSource>>() != null);

            var retval = new Collection<TSource>();

            foreach (TSource item in @this)
            {
                retval.Add(item);
            }

            return retval;
        }

        #endregion

        #region Aggregate Operators

        public static TAccumulate AggregateBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Acknowledge.Object(@this);
            Contract.Requires(accumulator != null);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static TSource AggregateBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator)
        {
            Acknowledge.Object(@this);
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

            TAccumulate retval = seed;

            using (var iter = @this.GetEnumerator())
            {
                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = accumulator.Invoke(retval, iter.Current);
                }
            }

            return retval;
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

                TSource retval = iter.Current;

                while (predicate.Invoke(retval) && iter.MoveNext())
                {
                    retval = accumulator.Invoke(retval, iter.Current);
                }

                return retval;
            }
        }

        #endregion
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods that depend on the <see cref="Maybe{T}"/> class..
    /// </content>
    public static partial class EnumerableExtensions
    {
        // Custom version of CollectCore.
        internal static Maybe<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.Object(@this);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the expected behaviour when m is null?
                if (!m.IsSome)
                {
                    return Maybe<IEnumerable<TSource>>.None;
                }

                list.Add(m.Value);
            }

            return Maybe.Of(list.AsEnumerable());
        }
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods that depend on the <see cref="Output{T}"/> class.
    /// </content>
    public static partial class EnumerableExtensions
    {
        // Custom version of CollectCore.
        internal static Output<IEnumerable<TSource>> CollectCore<TSource>(this IEnumerable<Output<TSource>> @this)
        {
            Require.Object(@this);
            Contract.Ensures(Contract.Result<Output<IEnumerable<TSource>>>() != null);

            var list = new List<TSource>();

            foreach (var m in @this)
            {
                // REVIEW: Is this the correct behaviour when m is null?
                if (m == null)
                {
                    continue;
                }

                if (!m.IsSuccess)
                {
                    return Output.Failure<IEnumerable<TSource>>(m.ToExceptionDispatchInfo());
                }

                list.Add(m.ToValue());
            }

            return Output.Success(list.AsEnumerable());
        }
    }
}
