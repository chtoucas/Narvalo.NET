namespace Narvalo.Benchmarks.Internal
{
    using System.Collections.Generic;

    internal class BenchComparator
    {
        readonly Benchmarker _benchmarker;

        public BenchComparator(Benchmarker benchmarker)
        {
            Requires.NotNull(benchmarker, "benchmarker");

            _benchmarker = benchmarker;
        }

        public IEnumerable<BenchMetric> Compare(BenchComparison comparison)
        {
            Requires.NotNull(comparison, "comparison");

            return Compare(comparison.Items, comparison.Iterations);
        }

        public IEnumerable<BenchMetric> Compare(
            IEnumerable<BenchComparative> items,
            int iterations)
        {
            Requires.NotNull(items, "items");

            foreach (var item in items) {
                var duration = _benchmarker.Time(item.Action, iterations);

                yield return new BenchMetric(item.Name, duration, iterations);
            }
        }
    }
}
