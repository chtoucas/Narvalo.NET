// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Benchmarks.Sdk
{
    using System.Globalization;

    public interface IBenchmarkMetricCollectionFormatter
    {
        string Format(BenchmarkMetricCollection metrics);

        string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics);
    }
}
