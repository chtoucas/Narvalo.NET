namespace Narvalo.Benchmark
{
    using Narvalo.Benchmark.Internal;

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
            return Create(new BenchTimer());
        }

        public static BenchmarkRunner Create(IBenchTimer timer)
        {
            return new BenchmarkRunner(new Benchmarker(timer));
        }

        #endregion

        public BenchMetric Run(Benchmark benchmark)
        {
            var duration = _benchmarker.Time(benchmark);

            return BenchMetric.Create(benchmark, duration);
        }
    }
}
