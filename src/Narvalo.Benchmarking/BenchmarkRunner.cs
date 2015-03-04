// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System.Diagnostics.Contracts;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkRunner
    {
        private readonly Benchmarker _benchmarker;

        private BenchmarkRunner(Benchmarker benchmarker)
        {
            Require.NotNull(benchmarker, "benchmarker");

            _benchmarker = benchmarker;
        }

        public static BenchmarkRunner Create()
        {
            return Create(new BenchmarkTimer());
        }

        public static BenchmarkRunner Create(IBenchmarkTimer timer)
        {
            Contract.Requires(timer != null);

            return new BenchmarkRunner(new Benchmarker(timer));
        }

        public BenchmarkMetric Run(Benchmark benchmark)
        {
            Contract.Requires(benchmark != null);

            var duration = _benchmarker.Time(benchmark);

            return BenchmarkMetric.Create(benchmark, duration);
        }
    }
}
