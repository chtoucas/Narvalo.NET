namespace Narvalo.Benchmark
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class BenchMetricFormatter : BenchMetricFormatterBase
    {
        public BenchMetricFormatter() : base() { }

        public override string FormatCore(CultureInfo cultureInfo, BenchMetric metric)
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

        public override string Format(CultureInfo cultureInfo, BenchMetricCollection metrics)
        {
            Require.NotNull(metrics, "metrics");

            var fastest = metrics.OrderBy(r => r.Duration).First();
            return String.Format(
                cultureInfo,
                SR.MetricFormat,
                metrics.Name,
                Format(cultureInfo, fastest));
        }

        public override string FormatInvalidMetric(CultureInfo cultureInfo, BenchMetric metric)
        {
            return String.Format(
                cultureInfo,
                SR.InvalidMetricFormat,
                metric.Iterations);
        }
    }
}
