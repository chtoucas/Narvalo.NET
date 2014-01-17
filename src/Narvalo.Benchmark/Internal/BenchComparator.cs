namespace Narvalo.Benchmark.Internal
{
    using System.Collections.Generic;

    class BenchComparator
    {
        readonly Benchmarker _benchmarker;

        public BenchComparator(Benchmarker benchmarker)
        {
            Require.NotNull(benchmarker, "benchmarker");

            _benchmarker = benchmarker;
        }

        public IEnumerable<BenchMetric> Compare(BenchComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            return Compare(comparison.Items, comparison.Iterations);
        }

        public IEnumerable<BenchMetric> Compare(
            IEnumerable<BenchComparative> items,
            int iterations)
        {
            Require.NotNull(items, "items");

            foreach (var item in items) {
                var duration = _benchmarker.Time(item.Action, iterations);

                yield return new BenchMetric(item.Name, duration, iterations);
            }
        }
    }
}
