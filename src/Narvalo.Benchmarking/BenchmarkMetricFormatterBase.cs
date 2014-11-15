namespace Narvalo.Benchmarking
{
    using System.Globalization;
    using NodaTime;

    public abstract class BenchmarkMetricFormatterBase : IBenchmarkMetricFormatter
    {
        protected BenchmarkMetricFormatterBase() { }

        #region IBenchMetricFormatter

        public string Format(BenchmarkMetric metric)
        {
            return Format(CultureInfo.CurrentCulture, metric);
        }

        public string Format(BenchmarkMetricCollection metrics)
        {
            return Format(CultureInfo.CurrentCulture, metrics);
        }

        public string Format(CultureInfo cultureInfo, BenchmarkMetric metric)
        {
            if (metric.Duration == Duration.Zero) {
                return FormatInvalidMetric(cultureInfo, metric);
            }
            return FormatCore(cultureInfo, metric);
        }

        public abstract string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics);

        #endregion

        public abstract string FormatCore(CultureInfo cultureInfo, BenchmarkMetric metric);

        public abstract string FormatInvalidMetric(CultureInfo cultureInfo, BenchmarkMetric metric);
    }
}
