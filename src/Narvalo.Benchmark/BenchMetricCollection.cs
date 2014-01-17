namespace Narvalo.Benchmark
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public class BenchMetricCollection : ReadOnlyCollection<BenchMetric>
    {
        readonly string _name;

        public BenchMetricCollection(
            string name,
            IList<BenchMetric> metrics)
            : base(metrics)
        {
            Require.NotNullOrEmpty(name, "name");

            _name = name;
        }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return ToString(BenchMetric.DefaultFormatter);
        }

        public string ToString(IBenchMetricFormatter fmt)
        {
            Require.NotNull(fmt, "fmt");

            return fmt.Format(CultureInfo.CurrentCulture, this);
        }
    }
}
