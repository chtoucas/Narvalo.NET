// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    /// <summary>
    /// Represents a processor that compares the performance of different implementations
    /// of the same problem given by methods defined inside a single class, each method
    /// being identified by its <see cref="BenchmarkComparative"/> attribute.
    /// </summary>
    public sealed class BenchmarkComparisonProcessor
    {
        private const BindingFlags DEFAULT_BINDINGS
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BenchmarkComparator _comparator;
        private readonly BenchmarkComparativeFinder _finder;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> with the 
        /// default bindings, used to look for methods with a benchmark attribute, 
        /// and with the default timer.
        /// </summary>
        public BenchmarkComparisonProcessor() : this(DEFAULT_BINDINGS, new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> with the 
        /// with the default bindings, used to look for methods with a benchmark attribute, 
        /// and with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkComparisonProcessor(IBenchmarkTimer timer) : this(DEFAULT_BINDINGS, timer) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> 
        /// with the specified bindings, used to look for methods with a benchmark attribute,
        /// and with the default timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        public BenchmarkComparisonProcessor(BindingFlags bindings) : this(bindings, new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> 
        /// with the specified binding, used  to look for methods with a benchmark attribute,
        /// and with the specified timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkComparisonProcessor(BindingFlags bindings, IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            _finder = new BenchmarkComparativeFinder(bindings);
            _comparator = new BenchmarkComparator(new BenchmarkRunner(timer));
        }

        /// <summary>
        /// Process a comparison of implementations found in <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type holding the implementations to be compared.</param>
        /// <returns>The collection of benchmark results, one result for each method run.</returns>
        public BenchmarkMetricCollection Process(Type type)
        {
            Contract.Requires(type != null);

            IEnumerable<BenchmarkComparative> items = _finder.FindComparatives(type);
            var comparison = CreateComparison_(type, items);
            IEnumerable<BenchmarkMetric> metrics = _comparator.Compare(comparison);

            return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
        }

        // REVIEW: Theory.
        // REVIEW: retourner plutôt un BenchMetricCollection ?
        public IEnumerable<BenchmarkMetricCollection> Process<T>(
            Type type,
            IEnumerable<T> testData)
        {
            Require.NotNull(testData, "testData");

            foreach (var value in testData) {
                // REVIEW: on peut sûrement éviter de relancer BenchComparisonFactory à chaque itération.
                IEnumerable<BenchmarkComparative> items = _finder.FindComparatives(type, value);
                var comparison = CreateComparison_(type, items);
                IEnumerable<BenchmarkMetric> metrics = _comparator.Compare(comparison);

                yield return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
            }
        }

        private static BenchmarkComparison CreateComparison_(Type type, IEnumerable<BenchmarkComparative> items)
        {
            Contract.Requires(type != null);

            BenchmarkComparisonAttribute attr 
                = type.GetCustomAttribute<BenchmarkComparisonAttribute>(inherit: false);

            if (attr == null)
            {
                var message = String.Format(
                    CultureInfo.CurrentCulture,
                    Strings_Benchmarking.MissingBenchComparisonAttribute,
                    type.Name);

                throw new BenchmarkException(message);
            }

            return new BenchmarkComparison(
                attr.DisplayName ?? type.Name,
                items,
                attr.Iterations);
        }
    }
}
