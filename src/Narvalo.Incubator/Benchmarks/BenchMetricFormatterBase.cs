namespace Narvalo.Benchmarks
{
    using System.Globalization;
    using NodaTime;

    public abstract class BenchMetricFormatterBase : IBenchMetricFormatter
    {
        protected BenchMetricFormatterBase() { }

        #region IBenchMetricFormatter

        public string Format(BenchMetric metric)
        {
            return Format(CultureInfo.CurrentCulture, metric);
        }

        public string Format(BenchMetricCollection metrics)
        {
            return Format(CultureInfo.CurrentCulture, metrics);
        }

        public string Format(CultureInfo cultureInfo, BenchMetric metric)
        {
            if (metric.Duration == Duration.Zero) {
                return FormatInvalidMetric(cultureInfo, metric);
            }
            return FormatCore(cultureInfo, metric);
        }

        public abstract string Format(CultureInfo cultureInfo, BenchMetricCollection metrics);

        #endregion

        public abstract string FormatCore(CultureInfo cultureInfo, BenchMetric metric);

        public abstract string FormatInvalidMetric(CultureInfo cultureInfo, BenchMetric metric);
    }
}
