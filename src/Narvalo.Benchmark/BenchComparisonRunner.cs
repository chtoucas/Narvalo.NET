namespace Narvalo.Benchmark
{
    using System.Linq;
    using Narvalo.Benchmark.Internal;
    //using Narvalo.Linq;

    public class BenchComparisonRunner
    {
        readonly BenchComparator _comparator;

        BenchComparisonRunner(BenchComparator comparator)
        {
            Require.NotNull(comparator, "comparator");

            _comparator = comparator;
        }

        #region Raccourcis de construction.

        public static BenchComparisonRunner Create()
        {
            return Create(new BenchTimer());
        }

        public static BenchComparisonRunner Create(IBenchTimer timer)
        {
            return new BenchComparisonRunner(new BenchComparator(new Benchmarker(timer)));
        }

        #endregion

        public BenchMetricCollection Run(BenchComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            var metrics = _comparator.Compare(comparison);

            return new BenchMetricCollection(comparison.Name, metrics.ToList());
        }
    }
}
