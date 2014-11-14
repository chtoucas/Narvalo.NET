namespace Narvalo.Runtime.Benchmarking
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class BenchmarkMetricCollection : ReadOnlyCollection<BenchmarkMetric>
    {
        readonly string _name;

        public BenchmarkMetricCollection(
            string name,
            IList<BenchmarkMetric> metrics)
            : base(metrics)
        {
            Require.NotNullOrEmpty(name, "name");

            _name = name;
        }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return ToString(BenchmarkMetric.DefaultFormatter);
        }

        public string ToString(IBenchmarkMetricFormatter fmt)
        {
            Require.NotNull(fmt, "fmt");

            return fmt.Format(CultureInfo.CurrentCulture, this);
        }
    }
}
