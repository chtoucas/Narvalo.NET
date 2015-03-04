// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public sealed class BenchmarkProcessor
    {
        private readonly BenchmarkFinder _finder;
        private readonly BenchmarkRunner _runner;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/>
        /// for the specified benchmark finder and benchmark runner.
        /// </summary>
        /// <param name="finder">The finder used to look for methods with a benchmark attribute.</param>
        /// <param name="runner">The benchmark runnner.</param>
        private BenchmarkProcessor(BenchmarkFinder finder, BenchmarkRunner runner)
        {
            Contract.Requires(finder != null);
            Contract.Requires(runner != null);

            _finder = finder;
            _runner = runner;
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkProcessor"/> class with the 
        /// default bindings, used to look for methods with a benchmark attribute, 
        /// and with the default timer.
        /// </summary>
        /// <returns>The default <see cref="BenchmarkProcessor"/> class.</returns>
        public static BenchmarkProcessor Create()
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(),
                BenchmarkRunner.Create());
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkProcessor"/> class 
        /// with the specified bindings, used to look for methods with a benchmark attribute,
        /// and with the default timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <returns>The <see cref="BenchmarkProcessor"/> for the specified bindings.</returns>
        public static BenchmarkProcessor Create(BindingFlags bindings)
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(bindings),
                BenchmarkRunner.Create());
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkProcessor"/> class 
        /// with the default bindings, used to look for methods with a benchmark attribute, 
        /// and with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        /// <returns>The <see cref="BenchmarkProcessor"/> for the specified timer.</returns>
        public static BenchmarkProcessor Create(IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            return new BenchmarkProcessor(
                new BenchmarkFinder(),
                BenchmarkRunner.Create(timer));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkProcessor"/> class
        /// with the specified binding, used  to look for methods with a benchmark attribute,
        /// and with the specified timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <param name="timer">The timer for measuring time intervals.</param>
        /// <returns>The <see cref="BenchmarkProcessor"/> for the specified bindings 
        /// and timer.</returns>
        public static BenchmarkProcessor Create(BindingFlags bindings, IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            return new BenchmarkProcessor(
                new BenchmarkFinder(bindings),
                BenchmarkRunner.Create(timer));
        }

        public IEnumerable<BenchmarkMetric> Process(Assembly assembly)
        {
            var benchmarks = _finder.FindBenchmarks(assembly);

            foreach (var benchmark in benchmarks) {
                yield return _runner.Run(benchmark);
            }
        }

        public IEnumerable<BenchmarkMetric> Process(Type type)
        {
            var benchmarks = _finder.FindBenchmarks(type);

            foreach (var benchmark in benchmarks) {
                yield return _runner.Run(benchmark);
            }
        }
    }
}
