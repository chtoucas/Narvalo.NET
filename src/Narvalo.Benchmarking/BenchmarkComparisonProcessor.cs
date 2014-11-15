namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public class BenchmarkComparisonProcessor
    {
        readonly BenchmarkComparativeFinder _finder;
        readonly BenchmarkComparisonRunner _runner;

        BenchmarkComparisonProcessor(
            BenchmarkComparativeFinder finder,
            BenchmarkComparisonRunner runner)
        {
            Require.NotNull(finder, "finder");
            Require.NotNull(runner, "runner");

            _finder = finder;
            _runner = runner;
        }

        #region Raccourcis de construction.

        public static BenchmarkComparisonProcessor Create()
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(),
                BenchmarkComparisonRunner.Create());
        }

        public static BenchmarkComparisonProcessor Create(BindingFlags bindings)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(bindings),
                BenchmarkComparisonRunner.Create());
        }

        public static BenchmarkComparisonProcessor Create(IBenchmarkTimer timer)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(), 
                BenchmarkComparisonRunner.Create(timer));
        }

        public static BenchmarkComparisonProcessor Create(
            BindingFlags bindings,
            IBenchmarkTimer timer)
        {
            return new BenchmarkComparisonProcessor(
                new BenchmarkComparativeFinder(bindings),
                BenchmarkComparisonRunner.Create(timer));
        }

        #endregion

        public BenchmarkMetricCollection Process(Type type)
        {
            var items = _finder.FindComparatives(type);
            var comparison = BenchmarkComparisonFactory.Create(type, items);

            return _runner.Run(comparison);
        }

        // FIXME: Theory.
        // FIXME: retourner plutôt un BenchMetricCollection ?
        public IEnumerable<BenchmarkMetricCollection> Process<T>(
            Type type,
            IEnumerable<T> testData)
        {
            foreach (var value in testData) {
                // FIXME: on peut sûrement éviter de relancer BenchComparisonFactory
                // FIXME: à chaque itération.
                var items = _finder.FindComparatives(type, value);
                var comparison = BenchmarkComparisonFactory.Create(type, items);

                yield return _runner.Run(comparison);
            }
        }
    }
}
