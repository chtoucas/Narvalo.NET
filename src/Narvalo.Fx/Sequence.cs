// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides a set of static methods to generate objects of type <see cref="IEnumerable{T}"/>.
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
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TError); }

                if (item.IsError) { yield return item.Error; }
            }
        }
    }

    // Provides extension methods for IEnumerable<Result<T, TError>>.
    public static partial class Sequence
    {
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
                // REVIEW: Is this the correct behaviour for null?
                if (item == null) { yield return default(TSource); }

                if (item.IsSuccess) { yield return item.Value; }
            }
        }
    }
}
