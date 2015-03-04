// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    public sealed class BenchmarkProcessor
    {
        private readonly Benchmarker _benchmarker;
        private readonly BenchmarkFinder _finder;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/>
        /// for the specified benchmark finder.
        /// </summary>
        /// <param name="finder">The finder used to look for methods with a benchmark attribute.</param>
        private BenchmarkProcessor(BenchmarkFinder finder) : this(finder, new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/>
        /// for the specified benchmark finder and timer.
        /// </summary>
        /// <param name="finder">The finder used to look for methods with a benchmark attribute.</param>
        /// <param name="timer">The timer for measuring time intervals.</param>
        private BenchmarkProcessor(BenchmarkFinder finder, IBenchmarkTimer timer)
        {
            Contract.Requires(finder != null);
            Contract.Requires(timer != null);

            _finder = finder;
            _benchmarker = new Benchmarker(timer);
        }

        /// <summary>
        /// Obtains an instance of the <see cref="BenchmarkProcessor"/> class with the 
        /// default bindings, used to look for methods with a benchmark attribute, 
        /// and with the default timer.
        /// </summary>
        /// <returns>The default <see cref="BenchmarkProcessor"/> class.</returns>
        public static BenchmarkProcessor Create()
        {
            return new BenchmarkProcessor(new BenchmarkFinder());
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
            return new BenchmarkProcessor(new BenchmarkFinder(bindings));
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

            return new BenchmarkProcessor(new BenchmarkFinder(), timer);
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

            return new BenchmarkProcessor(new BenchmarkFinder(bindings), timer);
        }

        public IEnumerable<BenchmarkMetric> Process(Assembly assembly)
        {
            var benchmarks = _finder.FindBenchmarks(assembly);

            foreach (var benchmark in benchmarks) {
                var duration = _benchmarker.Time(benchmark);

                yield return BenchmarkMetric.Create(benchmark, duration);
            }
        }

        public IEnumerable<BenchmarkMetric> Process(Type type)
        {
            var benchmarks = _finder.FindBenchmarks(type);

            foreach (var benchmark in benchmarks) {
                var duration = _benchmarker.Time(benchmark);

                yield return BenchmarkMetric.Create(benchmark, duration);
            }
        }
    }
}
