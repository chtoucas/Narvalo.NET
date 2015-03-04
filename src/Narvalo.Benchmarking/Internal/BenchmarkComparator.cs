// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System.Collections.Generic;

    using Narvalo;
    using Narvalo.Benchmarking;
    using NodaTime;

    internal sealed class BenchmarkComparator
    {
        private readonly BenchmarkRunner _runner;

        public BenchmarkComparator(BenchmarkRunner runner)
        {
            Require.NotNull(runner, "runner");

            _runner = runner;
        }

        public IEnumerable<BenchmarkMetric> Compare(BenchmarkComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            return Compare(comparison.Items, comparison.Iterations);
        }

        public IEnumerable<BenchmarkMetric> Compare(
            IEnumerable<BenchmarkComparative> items,
            int iterations)
        {
            Require.NotNull(items, "items");

            foreach (var item in items) {
                Duration duration = _runner.Time(item.Action, iterations);

                yield return new BenchmarkMetric(item.Name, duration, iterations);
            }
        }
    }
}
