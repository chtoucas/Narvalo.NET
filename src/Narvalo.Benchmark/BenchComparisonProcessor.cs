namespace Narvalo.Benchmark
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Narvalo.Benchmark.Internal;

    public class BenchComparisonProcessor
    {
        readonly BenchComparativeFinder _finder;
        readonly BenchComparisonRunner _runner;

        BenchComparisonProcessor(
            BenchComparativeFinder finder,
            BenchComparisonRunner runner)
        {
            Requires.NotNull(finder, "finder");
            Requires.NotNull(runner, "runner");

            _finder = finder;
            _runner = runner;
        }

        #region Raccourcis de construction.

        public static BenchComparisonProcessor Create()
        {
            return new BenchComparisonProcessor(
                new BenchComparativeFinder(),
                BenchComparisonRunner.Create());
        }

        public static BenchComparisonProcessor Create(BindingFlags bindings)
        {
            return new BenchComparisonProcessor(
                new BenchComparativeFinder(bindings),
                BenchComparisonRunner.Create());
        }

        public static BenchComparisonProcessor Create(IBenchTimer timer)
        {
            return new BenchComparisonProcessor(
                new BenchComparativeFinder(), 
                BenchComparisonRunner.Create(timer));
        }

        public static BenchComparisonProcessor Create(
            BindingFlags bindings,
            IBenchTimer timer)
        {
            return new BenchComparisonProcessor(
                new BenchComparativeFinder(bindings),
                BenchComparisonRunner.Create(timer));
        }

        #endregion

        public BenchMetricCollection Process(Type type)
        {
            var items = _finder.FindComparatives(type);
            var comparison = BenchComparisonFactory.Create(type, items);

            return _runner.Run(comparison);
        }

        // FIXME: Theory.
        // FIXME: retourner plutôt un BenchMetricCollection ?
        public IEnumerable<BenchMetricCollection> Process<T>(
            Type type,
            IEnumerable<T> testData)
        {
            foreach (var value in testData) {
                // FIXME: on peut sûrement de relancer BenchComparisonFactory
                // FIXME: à chaque itération.
                var items = _finder.FindComparatives(type, value);
                var comparison = BenchComparisonFactory.Create(type, items);

                yield return _runner.Run(comparison);
            }
        }
    }
}
