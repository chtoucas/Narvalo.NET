// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System.Globalization;

    using NodaTime;

    public abstract class BenchmarkMetricFormatterBase : IBenchmarkMetricFormatter
    {
        protected BenchmarkMetricFormatterBase() { }

        public string Format(BenchmarkMetric metric)
        {
            return Format(CultureInfo.CurrentCulture, metric);
        }

        public string Format(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            if (metric.Duration == Duration.Zero) {
                return FormatInvalidMetric(cultureInfo, metric);
            }

            return FormatCore(cultureInfo, metric);
        }

        public abstract string FormatCore(CultureInfo cultureInfo, BenchmarkMetric metric);

        public abstract string FormatInvalidMetric(CultureInfo cultureInfo, BenchmarkMetric metric);
    }
}
