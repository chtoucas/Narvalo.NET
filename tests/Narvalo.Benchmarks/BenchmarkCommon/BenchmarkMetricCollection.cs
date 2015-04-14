// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public sealed class BenchmarkMetricCollection : ReadOnlyCollection<BenchmarkMetric>
    {
        private readonly string _name;

        public BenchmarkMetricCollection(
            string name,
            IList<BenchmarkMetric> metrics)
            : base(metrics)
        {
            Require.NotNullOrEmpty(name, "name");
            Contract.Requires(metrics != null);

            _name = name;
        }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return ToString(new BenchmarkMetricCollectionFormatter());
        }

        public string ToString(IBenchmarkMetricCollectionFormatter formatter)
        {
            Require.NotNull(formatter, "formatter");
            Contract.Ensures(Contract.Result<string>() != null);

            return formatter.Format(CultureInfo.CurrentCulture, this);
        }
    }
}
