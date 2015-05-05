// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public static class Sequence<T>
    {
        //public static Func<int, T> AlwaysDefault
        //{
        //    get
        //    {
        //        Contract.Ensures(Contract.Result<Func<int, T>>() != null);

        //        return Stubs<int, T>.AlwaysDefault;
        //    }
        //}

        public static T AlwaysDefault(int i)
        {
            return default(T);
        }
    }

    public static class Sequence
    {
        private static readonly Func<int, int> s_Int32Increment = i => i + 1;

        private static readonly Func<long, long> s_Int64Increment = i => i + 1L;

        public static Func<int, int> Identity
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<int, int>>() != null);

                return Stubs<int>.Identity;
            }
        }

        public static Func<int, int> Increment
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<int, int>>() != null);

                return s_Int32Increment;
            }
        }

        public static Func<int, bool> AlwaysTrue
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<int, bool>>() != null);

                return Stubs<int>.AlwaysTrue;
            }
        }

        public static Func<int, bool> AlwaysFalse
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<int, bool>>() != null);

                return Stubs<int>.AlwaysFalse;
            }
        }

        public static long LongIncrement(long i)
        {
            return i + 1L;
        }

        /// <summary>
        /// Generates a sequence that contains exactly one value.
        /// </summary>
        /// <remarks>The result is immutable.</remarks>
        /// <typeparam name="TSource">The type of the value to be used in the result sequence.</typeparam>
        /// <param name="value">The single value of the sequence.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains a single element.</returns>
        public static IEnumerable<TSource> Single<TSource>(TSource value)
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // Enumerable.Repeat(value, 1) would work too, but is less readable.
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
            Contract.Requires(generator != null);
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return Unfold(seed, generator, Stubs<TSource>.AlwaysTrue);
        }

        public static IEnumerable<TResult> Unfold<TSource, TResult>(
            TSource seed,
            Func<TSource, Iteration<TResult, TSource>> generator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(generator, "generator");
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

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
        /// Generates an infinite sequence.
        /// </summary>
        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator)
        {
            Contract.Requires(iterator != null);
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            return Gather(seed, iterator, Stubs<TSource>.AlwaysTrue);
        }

        public static IEnumerable<TSource> Gather<TSource>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, bool> predicate)
        {
            Require.NotNull(iterator, "iterator");
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return current;

                current = iterator.Invoke(current);
            }
        }

        /// <summary>
        /// Generates an infinite sequence.
        /// </summary>
        /// <remarks>
        /// This method can be derived from the Unfold anamorphism:
        /// <code>
        /// Sequence.Unfold(seed, _ => Iteration.Create(resultSelector.Invoke(_), iterator.Invoke(_)));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Gather<TSource, TResult>(
            TSource seed,
            Func<TSource, TSource> iterator,
            Func<TSource, TResult> resultSelector)
        {
            Contract.Requires(iterator != null);
            Contract.Requires(resultSelector != null);
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            return Gather(seed, iterator, resultSelector, Stubs<TSource>.AlwaysTrue);
        }

        /// <remarks>
        /// This method can be derived from the Unfold anamorphism:
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
            Require.NotNull(iterator, "iterator");
            Require.NotNull(resultSelector, "resultSelector");
            Require.NotNull(predicate, "predicate");
            Contract.Ensures(Contract.Result<IEnumerable<TResult>>() != null);

            TSource current = seed;

            while (predicate.Invoke(current))
            {
                yield return resultSelector.Invoke(current);

                current = iterator.Invoke(current);
            }
        }

        #endregion

        /// <summary>
        /// Returns an empty sequence that has the specified type argument.
        /// </summary>
        /// <remarks>
        /// Workaround for the fact that <see cref="Enumerable.Empty{T}"/> does not have any contract attached.
        /// </remarks>
        /// <typeparam name="TSource">The type to assign to the type parameter of the returned
        /// generic <see cref="IEnumerable{T}"/>.</typeparam>
        /// <returns>An empty <see cref="IEnumerable{T}"/> whose type argument is TResult.</returns>
        internal static IEnumerable<TSource> Empty<TSource>()
        {
            Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

            // We could use "yield break", but Enumerable.Empty<T> is more readable
            // with the additional benefit of returning a singleton.
            var retval = Enumerable.Empty<TSource>();
            Contract.Assume(retval != null);

            return retval;
        }
    }
}
