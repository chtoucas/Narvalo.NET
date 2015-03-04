// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    using NodaTime;

    public sealed class BenchmarkProcessor
    {
        private const BindingFlags DEFAULT_BINDINGS
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BindingFlags _bindings;
        private readonly BenchmarkRunner _runner;

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
        public BenchmarkProcessor(IBenchmarkTimer timer)
            : this(DEFAULT_BINDINGS, timer)
        {
            Contract.Requires(timer != null);
        }

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

            _bindings = bindings;
            _runner = new BenchmarkRunner(timer);
        }

        public IEnumerable<BenchmarkMetric> Process(Assembly assembly)
        {
            Require.NotNull(assembly, "assembly");
            Contract.Ensures(Contract.Result<IEnumerable<BenchmarkMetric>>() != null);

            IEnumerable<Benchmark> benchmarks
                = from type in assembly.GetExportedTypes()
                  from benchmark in FindBenchmarks_(type)
                  select benchmark;

            foreach (var benchmark in benchmarks)
            {
                Duration duration = _runner.Run(benchmark);

                yield return new BenchmarkMetric(benchmark.Name, duration, benchmark.Iterations);
            }
        }

        public IEnumerable<BenchmarkMetric> Process(Type type)
        {
            Contract.Ensures(Contract.Result<IEnumerable<BenchmarkMetric>>() != null);

            IEnumerable<Benchmark> benchmarks = FindBenchmarks_(type);

            foreach (var benchmark in benchmarks)
            {
                Duration duration = _runner.Run(benchmark);

                yield return new BenchmarkMetric(benchmark.Name, duration, benchmark.Iterations);
            }
        }

        private IEnumerable<Benchmark> FindBenchmarks_(Type type)
        {
            MethodInfo[] methods = type.GetMethods(_bindings);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<BenchmarkAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                // FIXME: Cela ne marchera que si la méthode est statique.
                //var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                var action = (Action)method.CreateDelegate(typeof(Action));

                yield return new Benchmark(
                    attr.DisplayName ?? method.Name,
                    action,
                    attr.Iterations);
            }
        }
    }
}
