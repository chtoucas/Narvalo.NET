// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon
{
    using System;
    using System.Globalization;

    public class BenchmarkMetricFormatter : IBenchmarkMetricFormatter
    {
        public string Format(BenchmarkMetric metric)
        {
            return Format(CultureInfo.CurrentCulture, metric);
        }

        public string Format(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            if (metric.Duration == TimeSpan.Zero)
            {
                return String.Format(
                    cultureInfo,
                   "XXX",
                    metric.Iterations);
            }

            return String.Format(
                cultureInfo,
               "XXX",
                metric.Name,
                metric.CallsPerSecond,
                metric.Iterations,
                metric.Duration.Ticks,
                metric.TicksPerCall);
        }
    }
}
