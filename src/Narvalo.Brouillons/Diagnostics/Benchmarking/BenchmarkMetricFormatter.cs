// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Globalization;


    public class BenchmarkMetricFormatter : BenchmarkMetricFormatterBase
    {
        public BenchmarkMetricFormatter() : base() { }

        public override string FormatCore(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                Strings_Benchmarking.MetricFormat,
                metric.Name,
                metric.CallsPerSecond,
                metric.Iterations,
                metric.Duration.Ticks,
                metric.TicksPerCall);
        }

        public override string FormatInvalidMetric(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                Strings_Benchmarking.InvalidMetricFormat,
                metric.Iterations);
        }
    }
}
