// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Narvalo;
    using NodaTime;

    public class BenchmarkMetricCollectionFormatter : BenchmarkMetricCollectionFormatterBase
    {
        public BenchmarkMetricCollectionFormatter() : base() { }

        public override string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics)
        {
            Require.NotNull(metrics, "metrics");

            var fastest = metrics.OrderBy(r => r.Duration).First();

            return String.Format(
                cultureInfo,
                "{0}: {1:N0} cps; ({2:N0} iterations in {3:N0} ticks; {4:N0} ticks per iteration)",
                metrics.Name,
                Format(cultureInfo, fastest));
        }

        public string Format(BenchmarkMetric metric)
        {
            return Format(CultureInfo.CurrentCulture, metric);
        }

        public string Format(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            if (metric.Duration == Duration.Zero)
            {
                return FormatInvalidMetric(cultureInfo, metric);
            }

            return FormatCore(cultureInfo, metric);
        }

        public  string FormatCore(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                "{0}: {1:N0} cps; ({2:N0} iterations in {3:N0} ticks; {4:N0} ticks per iteration)",
                metric.Name,
                metric.CallsPerSecond,
                metric.Iterations,
                metric.Duration.Ticks,
                metric.TicksPerCall);
        }

        public  string FormatInvalidMetric(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                "Invalid result: duration was 0 ({0} iterations)",
                metric.Iterations);
        }
    }
}
