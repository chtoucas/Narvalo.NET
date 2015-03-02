// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkComparisonProcessor
    {
        private readonly BenchmarkComparativeFinder _finder;
        private readonly BenchmarkComparisonRunner _runner;

        private BenchmarkComparisonProcessor(
            BenchmarkComparativeFinder finder,
            BenchmarkComparisonRunner runner)
        {
            Require.NotNull(finder, "finder");
            Require.NotNull(runner, "runner");

            _finder = finder;
            _runner = runner;
        }

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

        public BenchmarkMetricCollection Process(Type type)
        {
            var items = _finder.FindComparatives(type);
            var comparison = BenchmarkComparisonFactory.Create(type, items);

            return _runner.Run(comparison);
        }

        // REVIEW: Theory.
        // REVIEW: retourner plutôt un BenchMetricCollection ?
        public IEnumerable<BenchmarkMetricCollection> Process<T>(
            Type type,
            IEnumerable<T> testData)
        {
            foreach (var value in testData) {
                // REVIEW: on peut sûrement éviter de relancer BenchComparisonFactory à chaque itération.
                var items = _finder.FindComparatives(type, value);
                var comparison = BenchmarkComparisonFactory.Create(type, items);

                yield return _runner.Run(comparison);
            }
        }
    }
}
