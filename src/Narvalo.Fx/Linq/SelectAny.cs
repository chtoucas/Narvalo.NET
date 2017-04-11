// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Linq
{
    using System;
    using System.Collections.Generic;

    public static partial class Qperators
    {
        /// <summary>
        /// Projects each element of a sequence into a new form while discarding any null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
            where TResult : class
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            // Identical to: source.Select(selector).Where(x => x != null);
            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result != null) { yield return result; }
                }
            }
        }

        /// <summary>
        /// Projects each element of a sequence into a new form while discarding any null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter
        /// of the function represents the index of the source element.</param>
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TResult> selector)
            where TResult : class
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            // Identical to: source.Select(selector).Where(x => x != null);
            IEnumerable<TResult> iterator()
            {
                int index = 0;
                foreach (var item in source)
                {
                    var result = selector(item, index);

                    if (result != null) { yield return result; }

                    index++;
                }
            }
        }

        /// <summary>
        /// Projects each element of a sequence into a new form while discarding any null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult?> selector)
            where TResult : struct
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            // Identical to: source.Select(selector).Where(x => x.HasValue).Select(x => x.Value);
            IEnumerable<TResult> iterator()
            {
                foreach (var item in source)
                {
                    var result = selector(item);

                    if (result.HasValue) { yield return result.Value; }
                }
            }
        }

        /// <summary>
        /// Projects each element of a sequence into a new form while discarding any null.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter
        /// of the function represents the index of the source element.</param>
        public static IEnumerable<TResult> SelectAny<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, int, TResult?> selector)
            where TResult : struct
        {
            Require.NotNull(source, nameof(source));
            Require.NotNull(selector, nameof(selector));

            return iterator();

            // Identical to: source.Select(selector).Where(x => x.HasValue).Select(x => x.Value);
            IEnumerable<TResult> iterator()
            {
                int index = 0;
                foreach (var item in source)
                {
                    var result = selector(item, index);

                    if (result.HasValue) { yield return result.Value; }

                    index++;
                }
            }
        }
    }
}
