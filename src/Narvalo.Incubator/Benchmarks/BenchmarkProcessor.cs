namespace Narvalo.Benchmarks
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Narvalo.Benchmarks.Internal;

    public class BenchmarkProcessor
    {
        readonly BenchmarkFinder _finder;
        readonly BenchmarkRunner _runner;

        BenchmarkProcessor(BenchmarkFinder finder, BenchmarkRunner runner)
        {
            Requires.NotNull(finder, "finder");
            Requires.NotNull(runner, "runner");

            _finder = finder;
            _runner = runner;
        }

        #region Raccourcis de construction.

        public static BenchmarkProcessor Create()
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(),
                BenchmarkRunner.Create());
        }

        public static BenchmarkProcessor Create(BindingFlags bindings)
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(bindings),
                BenchmarkRunner.Create());
        }

        public static BenchmarkProcessor Create(IBenchTimer timer)
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(),
                BenchmarkRunner.Create(timer));
        }

        public static BenchmarkProcessor Create(
            BindingFlags bindings,
            IBenchTimer timer)
        {
            return new BenchmarkProcessor(
                new BenchmarkFinder(bindings),
                BenchmarkRunner.Create(timer));
        }

        #endregion

        public IEnumerable<BenchMetric> Process(Assembly assembly)
        {
            var benchmarks = _finder.FindBenchmarks(assembly);

            foreach (var benchmark in benchmarks) {
                yield return _runner.Run(benchmark);
            }
        }

        public IEnumerable<BenchMetric> Process(Type type)
        {
            var benchmarks = _finder.FindBenchmarks(type);

            foreach (var benchmark in benchmarks) {
                yield return _runner.Run(benchmark);
            }
        }
    }
}
