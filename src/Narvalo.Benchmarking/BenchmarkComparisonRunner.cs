// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System.Linq;

    using Narvalo;
    using Narvalo.Benchmarking.Internal;

    public sealed class BenchmarkComparisonRunner
    {
        private readonly BenchmarkComparator _comparator;
        
        public BenchmarkComparisonRunner() : this(new BenchmarkTimer()) { }

        public BenchmarkComparisonRunner(IBenchmarkTimer timer)
        {
            Require.NotNull(timer, "timer");

            _comparator = new BenchmarkComparator(new Benchmarker(timer));
        }

        public BenchmarkMetricCollection Run(BenchmarkComparison comparison)
        {
            Require.NotNull(comparison, "comparison");

            var metrics = _comparator.Compare(comparison);

            return new BenchmarkMetricCollection(comparison.Name, metrics.ToList());
        }
    }
}
