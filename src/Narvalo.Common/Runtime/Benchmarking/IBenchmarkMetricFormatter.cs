namespace Narvalo.Runtime.Benchmarking
{
    using System.Globalization;

    public interface IBenchmarkMetricFormatter
    {
        string Format(BenchmarkMetric metric);

        string Format(BenchmarkMetricCollection metrics);

        string Format(CultureInfo cultureInfo, BenchmarkMetric metric);

        string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics);
    }
}
