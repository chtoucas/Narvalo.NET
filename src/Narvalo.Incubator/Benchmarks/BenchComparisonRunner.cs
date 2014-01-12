namespace Narvalo.Benchmarks
{
    using Narvalo.Benchmarks.Internal;
    using Narvalo.Collections;

    public class BenchComparisonRunner
    {
        readonly BenchComparator _comparator;

        BenchComparisonRunner(BenchComparator comparator)
        {
            Requires.NotNull(comparator, "comparator");

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
            Requires.NotNull(comparison, "comparison");

            var metrics = _comparator.Compare(comparison);

            return new BenchMetricCollection(comparison.Name, metrics.ToList());
        }
    }
}
