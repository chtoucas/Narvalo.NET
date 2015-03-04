// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground.Benchmarks.Sdk
{
    using System.Globalization;

    public abstract class BenchmarkMetricCollectionFormatterBase : IBenchmarkMetricCollectionFormatter
    {
        protected BenchmarkMetricCollectionFormatterBase() { }

        public string Format(BenchmarkMetricCollection metrics)
        {
            return Format(CultureInfo.CurrentCulture, metrics);
        }

        public abstract string Format(CultureInfo cultureInfo, BenchmarkMetricCollection metrics);
    }
}
