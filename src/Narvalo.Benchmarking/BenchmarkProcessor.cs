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
        /// <summary>
        /// Default bindings, used to look for methods with a benchmark attribute.
        /// </summary>
        public const BindingFlags DefaultDiscoveryBindings
           = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        private readonly BenchmarkRunner _runner;

        private BindingFlags _discoveryBindings = DefaultDiscoveryBindings;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> with the default timer.
        /// </summary>
        public BenchmarkProcessor() : this(new BenchmarkTimer()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="BenchmarkProcessor"/> with the specified timer.
        /// </summary>
        /// <param name="timer">The timer for measuring time intervals.</param>
        public BenchmarkProcessor(IBenchmarkTimer timer)
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

        public Duration TestDuration
        {
            get
            {
                Contract.Ensures(Contract.Result<Duration>().Ticks > 0L);
                return _runner.TestDuration;
            }

            set
            {
                Require.Predicate(value.Ticks > 0L, "value");
                _runner.TestDuration = value;
            }
        }

        public Duration WarmUpDuration
        {
            get
            {
                Contract.Ensures(Contract.Result<Duration>().Ticks > 0L);
                return _runner.WarmUpDuration;
            }

            set
            {
                Require.Predicate(value.Ticks > 0L, "value");
                _runner.WarmUpDuration = value;
            }
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
                yield return _runner.Run(benchmark);
            }
        }

        public IEnumerable<BenchmarkMetric> Process(Type type)
        {
            Contract.Ensures(Contract.Result<IEnumerable<BenchmarkMetric>>() != null);

            IEnumerable<Benchmark> benchmarks = FindBenchmarks_(type);

            foreach (var benchmark in benchmarks)
            {
                yield return _runner.Run(benchmark);
            }
        }

        private IEnumerable<Benchmark> FindBenchmarks_(Type type)
        {
            MethodInfo[] methods = type.GetMethods(DiscoveryBindings);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<BenchmarkAttribute>(inherit: false);

                if (attr == null)
                {
                    continue;
                }

                // FIXME: Cela ne marche que si la méthode est statique.
                // var action = (Action)Delegate.CreateDelegate(typeof(Action), method);
                var action = (Action)method.CreateDelegate(typeof(Action));

                yield return new Benchmark(
                   method.DeclaringType.FullName,
                   attr.DisplayName ?? method.Name,
                   action);
            }
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_runner != null);
        }

#endif
    }
}
