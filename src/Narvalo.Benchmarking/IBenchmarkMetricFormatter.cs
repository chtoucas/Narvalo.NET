// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
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
