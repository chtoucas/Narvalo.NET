// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Narvalo;
    using NodaTime;

    /// <summary>
    /// Represents a processor that compares the performance of different implementations
    /// of the same problem given by methods defined inside a single class, each method
    /// being identified by its <see cref="Benchmark"/> attribute.
    /// </summary>
    public sealed class BenchmarkComparisonProcessor
    {
        /// <summary>
        /// Default bindings, used to look for methods with a benchmark attribute.
        /// </summary>
        public const BindingFlags DefaultDiscoveryBindings
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BenchmarkRunner _runner;

        private BindingFlags _discoveryBindings = DefaultDiscoveryBindings;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> with the default timer.
        /// </summary>
        public BenchmarkComparisonProcessor() : this(new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkComparisonProcessor"/> with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkComparisonProcessor(IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            _runner = new BenchmarkRunner(timer);
        }

        // Gets or sets the bindings used to look for methods with a benchmark attribute.
        public BindingFlags DiscoveryBindings
        {
            get { return _discoveryBindings; }
            set { _discoveryBindings = value; }
        }

        /// <summary>
        /// Process a comparison of implementations found in <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type holding the implementations to be compared.</param>
        /// <returns>The collection of benchmark results, one result for each method run.</returns>
        public BenchmarkMetricCollection Process(Type type)
        {
            IEnumerable<Benchmark> items = FindComparatives_(type);
            var comparison = CreateComparison_(type, items);
            IEnumerable<BenchmarkMetric> metrics = ProcessCore_(comparison);

            return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
        }

        // REVIEW: Theory.
        // REVIEW: retourner plutôt un BenchMetricCollection ?
        public IEnumerable<BenchmarkMetricCollection> Process<T>(
            Type type,
            IEnumerable<T> testData)
        {
            Require.NotNull(testData, "testData");

            foreach (var value in testData)
            {
                // REVIEW: on peut sûrement éviter de relancer CreateComparison_ à chaque itération.
                IEnumerable<Benchmark> items = FindComparatives_(type, value);
                var comparison = CreateComparison_(type, items);
                IEnumerable<BenchmarkMetric> metrics = ProcessCore_(comparison);

                yield return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
            }
        }

        private IEnumerable<BenchmarkMetric> ProcessCore_(BenchmarkComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            foreach (var item in comparison.Items)
            {
                yield return _runner.Run(item, comparison.Iterations);
            }
        }

        private static BenchmarkComparison CreateComparison_(Type type, IEnumerable<Benchmark> items)
        {
            BenchmarkComparisonAttribute attr
                = type.GetCustomAttribute<BenchmarkComparisonAttribute>(inherit: false);

            if (attr == null)
            {
                var message = String.Format(
                    CultureInfo.CurrentCulture,
                    "XXX",
                    type.Name);

                throw new BenchmarkException(message);
            }

            return new BenchmarkComparison(
                attr.DisplayName ?? type.Name,
                items,
                attr.Iterations);
        }

        private IEnumerable<Benchmark> FindComparatives_(Type type)
        {
            MethodInfo[] methods = type.GetMethods(DiscoveryBindings);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<BenchmarkComparativeAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                // FIXME: Cela ne marchera que si la méthode est statique.
                var action = (Action)method.CreateDelegate(typeof(Action));

                yield return new Benchmark(
                    type.FullName,
                    attr.DisplayName ?? method.Name,
                    action);
            }
        }

        // FIXME: Theory.
        private IEnumerable<Benchmark> FindComparatives_<T>(Type type, T value)
        {
            MethodInfo[] methods = type.GetMethods(DiscoveryBindings);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<BenchmarkComparativeAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                yield return new Benchmark(
                    type.FullName,
                    attr.DisplayName ?? method.Name,
                    // FIXME: Cela ne marchera que si la méthode est statique.
                    () => ((Action<T>)Delegate.CreateDelegate(typeof(Action<T>), method)).Invoke(value));
            }
        }
    }
}
