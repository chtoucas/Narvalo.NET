// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking.Internal
{
    using System.Collections.Generic;

    using Narvalo;
    using Narvalo.Benchmarking;

    internal class BenchmarkComparator
    {
        private readonly Benchmarker _benchmarker;

        public BenchmarkComparator(Benchmarker benchmarker)
        {
            Require.NotNull(benchmarker, "benchmarker");

            _benchmarker = benchmarker;
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
                var duration = _benchmarker.Time(item.Action, iterations);

                yield return new BenchmarkMetric(item.Name, duration, iterations);
            }
        }
    }
}
