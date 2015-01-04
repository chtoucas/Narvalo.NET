// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public class BenchmarkRunner
    {
        readonly Benchmarker _benchmarker;

        BenchmarkRunner(Benchmarker benchmarker)
        {
            Require.NotNull(benchmarker, "benchmarker");

            _benchmarker = benchmarker;
        }

        #region Raccourcis de construction.

        public static BenchmarkRunner Create()
        {
            return Create(new BenchmarkTimer());
        }

        public static BenchmarkRunner Create(IBenchmarkTimer timer)
        {
            return new BenchmarkRunner(new Benchmarker(timer));
        }

        #endregion

        public BenchmarkMetric Run(Benchmark benchmark)
        {
            var duration = _benchmarker.Time(benchmark);

            return BenchmarkMetric.Create(benchmark, duration);
        }
    }
}
