namespace Narvalo.Runtime.Benchmarking
{
    using System.Linq;
    using Narvalo.Internal;

    public class BenchmarkComparisonRunner
    {
        readonly BenchmarkComparator _comparator;

        BenchmarkComparisonRunner(BenchmarkComparator comparator)
        {
            Require.NotNull(comparator, "comparator");

            _comparator = comparator;
        }

        #region Raccourcis de construction.

        public static BenchmarkComparisonRunner Create()
        {
            return Create(new BenchmarkTimer());
        }

        public static BenchmarkComparisonRunner Create(IBenchmarkTimer timer)
        {
            return new BenchmarkComparisonRunner(new BenchmarkComparator(new Benchmarker(timer)));
        }

        #endregion

        public BenchmarkMetricCollection Run(BenchmarkComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            var metrics = _comparator.Compare(comparison);

            return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
        }
    }
}
