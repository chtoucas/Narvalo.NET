// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a set of static and extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static partial class Sequence
    {
        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Of<TSource>(TSource value)
        {
            Warrant.NotNull<IEnumerable<TSource>>();

            // Enumerable.Repeat(value, 1) works too, but is less readable.
            yield return value;
        }

        #region Anamorphisms

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator)
        {
            Require.NotNull(generator, nameof(generator));
            Warrant.NotNull<IEnumerable<TResult>>();

            return UnfoldIterator(seed, generator);
        }

        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, nameof(generator));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TResult>>();

            return UnfoldIterator(seed, generator, predicate);
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator)
        {
            Demand.NotNull(generator);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (true)
            {
                var iter = generator.Invoke(current);

                yield return iter.Result;

                current = iter.Next;
            }
        }

        private static IEnumerable<TResult> UnfoldIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(generator);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                var iter = generator.Invoke(current);

                yield return iter.Result;

                current = iter.Next;
            }
        }

        #endregion

        #region List Comprehensions

        /// <summary>
        /// Generates an infinite sequence containing one repeated value.
        /// </summary>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The value to be repeated.</param>
        public static IEnumerable<TSource> Gather<TSource>(TSource value)
        {
            while (true)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Require.NotNull(iterator, nameof(iterator));
            Warrant.NotNull<IEnumerable<TSource>>();

            return GatherIterator(seed, iterator);
        }

        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TSource>>();

            return GatherIterator(seed, iterator, predicate);
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => Iteration.Create(resultSelector.Invoke(_), iterator.Invoke(_)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Warrant.NotNull<IEnumerable<TResult>>();

            return GatherIterator(seed, iterator, resultSelector);
        }

        /// <remarks>
        /// This method can be derived from Unfold:
        /// <code>
        /// Sequence.Unfold(seed, _ => Iteration.Create(resultSelector.Invoke(_), iterator.Invoke(_)), predicate);
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, nameof(iterator));
            Require.NotNull(resultSelector, nameof(resultSelector));
            Require.NotNull(predicate, nameof(predicate));
            Warrant.NotNull<IEnumerable<TResult>>();

            return GatherIterator(seed, iterator, resultSelector, predicate);
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Demand.NotNull(iterator);
            Warrant.NotNull<IEnumerable<TSource>>();

            TSource current = seed;

            while (true)
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        private static IEnumerable<TSource> GatherIterator<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TSource>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(resultSelector);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (true)
            {
                yield return resultSelector.Invoke(current);

                current = iterator.Invoke(current);
            }
        }

        private static IEnumerable<TResult> GatherIterator<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector,
            Func<TSource, bool> predicate)
        {
            Demand.NotNull(iterator);
            Demand.NotNull(resultSelector);
            Demand.NotNull(predicate);
            Warrant.NotNull<IEnumerable<TResult>>();

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return resultSelector.Invoke(current);

                current = iterator.Invoke(current);
            }
        }

        #endregion
    }

    // Provides additional LINQ extensions.
    public static partial class Sequence
    {
        // Useful when using built-in LINQ operators. Even if it is not publicly visible,
        // I believe that all LINQ operators never return a null but rather an empty sequence if needed.
        public static IEnumerable<TSource> EmptyIfNull<TSource>(this IEnumerable<TSource> @this)
        {
            Warrant.NotNull<IEnumerable<TSource>>();

            if (@this == null)
            {
                return Enumerable.Empty<TSource>();
            }

            return @this;
        }

        public static bool IsEmpty<TSource>(this IEnumerable<TSource> @this)
        {
            Expect.NotNull(@this);

            return !@this.Any();
        }

        #region Element Operators

        // Named <c>listToMaybe</c> in Haskell parlance.
        public static Maybe<TSource> FirstOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (var iter = @this.GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> FirstOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            var seq = from t in @this where predicate.Invoke(t) select t;

            using (var iter = seq.EmptyIfNull().GetEnumerator())
            {
                return iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Expect.NotNull(@this);

            return @this.Reverse().EmptyIfNull().FirstOrNone();
        }

        public static Maybe<TSource> LastOrNone<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> predicate)
        {
            Expect.NotNull(@this);
            Expect.NotNull(predicate);

            return @this.Reverse().EmptyIfNull().FirstOrNone(predicate);
        }

        public static Maybe<TSource> SingleOrNone<TSource>(this IEnumerable<TSource> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (var iter = @this.EmptyIfNull().GetEnumerator())
            {
                // Return None if the sequence is empty.
                var result = iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        public static Maybe<TSource> SingleOrNone<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(predicate, nameof(predicate));

            var seq = from t in @this where predicate.Invoke(t) select t;

            using (var iter = seq.EmptyIfNull().GetEnumerator())
            {
                // Return None if the sequence is empty.
                var result = iter.MoveNext() ? Maybe.Of(iter.Current) : Maybe<TSource>.None;

                // Return None if there is one more element.
                return iter.MoveNext() ? Maybe<TSource>.None : result;
            }
        }

        #endregion

        #region Concatenation Operators

        // There is a much better implementation coming soon (?).
        // https://github.com/dotnet/corefx/commits/master/src/System.Linq/src/System/Linq/AppendPrepend.cs
        // REVIEW: Since this method is a critical component of Collect(), we could include the
        // .NET implementation until it is publicly available.
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return AppendIterator(@this, element);
        }

        private static IEnumerable<TSource> AppendIterator<TSource>(IEnumerable<TSource> source, TSource element)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                yield return item;
            }

            yield return element;
        }

        // See remarks for Append.
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> @this, TSource element)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return PrependIterator(@this, element);
        }

        private static IEnumerable<TSource> PrependIterator<TSource>(IEnumerable<TSource> source, TSource element)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            yield return element;

            foreach (var item in source)
            {
                yield return item;
            }
        }

        #endregion

        #region Aggregate Operators

        public static TAccumulate AggregateBack<TSource, TAccumulate>(
            this IEnumerable<TSource> @this,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

            return @this.Reverse().Aggregate(seed, accumulator);
        }

        public static TSource AggregateBack<TSource>(
            this IEnumerable<TSource> @this,
            Func<TSource, TSource, TSource> accumulator)
        {
            Expect.NotNull(@this);
            Expect.NotNull(accumulator);

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
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

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
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(accumulator, nameof(accumulator));
            Require.NotNull(predicate, nameof(predicate));

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

    // Provides extension methods for IEnumerable<Maybe<T>>.
    public static partial class Sequence
    {
        // Named <c>catMaybes</c> in Haskell parlance.
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Maybe<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Maybe<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                if (item.IsSome) { yield return item.Value; }
            }
        }
    }

    // Provides extension methods for IEnumerable<Outcome<T>>.
    public static partial class Sequence
    {
        public static IEnumerable<TSource> CollectAny<TSource>(this IEnumerable<Outcome<TSource>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource>(IEnumerable<Outcome<TSource>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TSource); }

                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }

    // Provides extension methods for IEnumerable<VoidOr<TError>>.
    public static partial class Sequence
    {
        public static IEnumerable<TError> CollectAny<TError>(this IEnumerable<VoidOr<TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TError>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TError> CollectAnyIterator<TError>(IEnumerable<VoidOr<TError>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TError>>();

            foreach (var item in source)
            {
                if (item.IsError) { yield return item.Error; }
            }
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class Sequence
    {
        public static Result<IEnumerable<TSource>, TError> Collect<TSource, TError>(
            this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));

            return Result.Of<IEnumerable<TSource>, TError>(CollectAnyIterator(@this));
        }

        public static IEnumerable<TSource> CollectAny<TSource, TError>(this IEnumerable<Result<TSource, TError>> @this)
        {
            Require.NotNull(@this, nameof(@this));
            Warrant.NotNull<IEnumerable<TSource>>();

            return CollectAnyIterator(@this);
        }

        internal static IEnumerable<TSource> CollectAnyIterator<TSource, TError>(IEnumerable<Result<TSource, TError>> source)
        {
            Demand.NotNull(source);
            Warrant.NotNull<IEnumerable<TSource>>();

            foreach (var item in source)
            {
                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
