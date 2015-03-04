// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using Narvalo;

    /// <summary>
    /// Represents a processor that compares the performance of different implementations
    /// of the same problem given by methods defined inside a single class, each method
    /// being identified by its <see cref="BenchmarkComparative"/> attribute.
    /// </summary>
    public sealed class BenchmarkComparisonProcessor
    {
        private readonly BenchmarkComparativeFinder _finder;
        private readonly BenchmarkComparisonRunner _runner;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/>
        /// for the specified implementations finder and benchmark runner.
        /// </summary>
        /// <param name="finder">The finder used to look for methods with a benchmark attribute.</param>
        /// <param name="runner">The benchmark runnner.</param>
        private BenchmarkComparisonProcessor(
            BenchmarkComparativeFinder finder,
            BenchmarkComparisonRunner runner)
        {
            Contract.Requires(finder != null);
            Contract.Requires(runner != null);

            _finder = finder;
            _runner = runner;
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkComparisonProcessor"/> class with the 
        /// default bindings, used to look for methods with a benchmark attribute, 
        /// and with the default timer.
        /// </summary>
        /// <returns>The default <see cref="BenchmarkComparisonProcessor"/> class.</returns>
        public static BenchmarkComparisonProcessor Create()
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(),
                BenchmarkComparisonRunner.Create());
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkComparisonProcessor"/> class 
        /// with the specified bindings, used to look for methods with a benchmark attribute,
        /// and with the default timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <returns>The <see cref="BenchmarkComparisonProcessor"/> for the specified bindings.</returns>
        public static BenchmarkComparisonProcessor Create(BindingFlags bindings)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(bindings),
                BenchmarkComparisonRunner.Create());
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkComparisonProcessor"/> class 
        /// with the default bindings, used to look for methods with a benchmark attribute, 
        /// and with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        /// <returns>The <see cref="BenchmarkComparisonProcessor"/> for the specified timer.</returns>
        public static BenchmarkComparisonProcessor Create(IBenchmarkTimer timer)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(), 
                BenchmarkComparisonRunner.Create(timer));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkComparisonProcessor"/> class
        /// with the specified binding, used  to look for methods with a benchmark attribute,
        /// and with the specified timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <param name="timer">The timer for measuring time intervals.</param>
        /// <returns>The <see cref="BenchmarkComparisonProcessor"/> for the specified bindings 
        /// and timer.</returns>
        public static BenchmarkComparisonProcessor Create(
            BindingFlags bindings,
            IBenchmarkTimer timer)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(bindings),
                BenchmarkComparisonRunner.Create(timer));
        }

        /// <summary>
        /// Process a comparison of implementations found in <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type holding the implementations to be compared.</param>
        /// <returns>The collection of benchmark results, one result for each method run.</returns>
        public BenchmarkMetricCollection Process(Type type)
        {
            IEnumerable<BenchmarkComparative> items = _finder.FindComparatives(type);
            var comparison = BenchmarkComparison.Create(type, items);

            return _runner.Run(comparison);
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
                var comparison = BenchmarkComparison.Create(type, items);

                yield return _runner.Run(comparison);
            }
        }
    }
}
