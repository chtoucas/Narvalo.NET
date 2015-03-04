// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    using NodaTime;

    public sealed class BenchmarkProcessor
    {
        private const BindingFlags DEFAULT_BINDINGS
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BenchmarkRunner _runner;
        private readonly BenchmarkFinder _finder;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> with the 
        /// default bindings, used to look for methods with a benchmark attribute, 
        /// and with the default timer.
        /// </summary>
        public BenchmarkProcessor() : this(DEFAULT_BINDINGS, new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> with the 
        /// with the default bindings, used to look for methods with a benchmark attribute, 
        /// and with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkProcessor(IBenchmarkTimer timer) : this(DEFAULT_BINDINGS, timer) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> 
        /// with the specified bindings, used to look for methods with a benchmark attribute,
        /// and with the default timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        public BenchmarkProcessor(BindingFlags bindings) : this(bindings, new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> 
        /// with the specified binding, used  to look for methods with a benchmark attribute,
        /// and with the specified timer.
        /// </summary>
        /// <param name="bindings">The bindings used to look for methods with a benchmark attribute.</param>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkProcessor(BindingFlags bindings, IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            _finder = new BenchmarkFinder(bindings);
            _runner = new BenchmarkRunner(timer);
        }

        public IEnumerable<BenchmarkMetric> Process(Assembly assembly)
        {
            var benchmarks = _finder.FindBenchmarks(assembly);

            foreach (var benchmark in benchmarks)
            {
                Duration duration = _runner.Run(benchmark);

                yield return BenchmarkMetric.Create(benchmark, duration);
            }
        }

        public IEnumerable<BenchmarkMetric> Process(Type type)
        {
            var benchmarks = _finder.FindBenchmarks(type);

            foreach (var benchmark in benchmarks)
            {
                Duration duration = _runner.Run(benchmark);

                yield return BenchmarkMetric.Create(benchmark, duration);
            }
        }
    }
}
