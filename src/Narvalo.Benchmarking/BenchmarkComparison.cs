namespace Narvalo.Benchmarking
{
    using System.Collections.Generic;
    using Narvalo;

    public class BenchmarkComparison
    {
        readonly IEnumerable<BenchmarkComparative> _items;
        readonly int _iterations;
        readonly string _name;

        public BenchmarkComparison(
            string name,
            IEnumerable<BenchmarkComparative> items,
            int iterations)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(items, "items");

            _name = name;
            _items = items;
            _iterations = iterations;
        }

        public IEnumerable<BenchmarkComparative> Items { get { return _items; } }

        public int Iterations { get { return _iterations; } }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
