namespace Narvalo.Benchmarking
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Narvalo;

    public class BenchmarkMetricFormatter : BenchmarkMetricFormatterBase
    {
        public BenchmarkMetricFormatter() : base() { }

        public override string FormatCore(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                SR.MetricFormat,
                metric.Name,
                metric.CallsPerSecond,
                metric.Iterations,
                metric.Duration.Ticks,
                metric.TicksPerCall);
        }

        public override string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics)
        {
            Require.NotNull(metrics, "metrics");

            var fastest = metrics.OrderBy(r => r.Duration).First();
            return String.Format(
                cultureInfo,
                SR.MetricFormat,
                metrics.Name,
                Format(cultureInfo, fastest));
        }

        public override string FormatInvalidMetric(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            return String.Format(
                cultureInfo,
                SR.InvalidMetricFormat,
                metric.Iterations);
        }
    }
}
