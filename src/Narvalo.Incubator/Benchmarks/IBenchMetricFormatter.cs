namespace Narvalo.Benchmarks
{
    using System.Globalization;

    public interface IBenchMetricFormatter
    {
        string Format(BenchMetric metric);

        string Format(BenchMetricCollection metrics);

        string Format(CultureInfo cultureInfo, BenchMetric metric);

        string Format(CultureInfo cultureInfo, BenchMetricCollection metrics);
    }
}
