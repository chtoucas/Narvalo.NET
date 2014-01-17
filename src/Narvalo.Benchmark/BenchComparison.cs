namespace Narvalo.Benchmark
{
    using System.Collections.Generic;

    public class BenchComparison
    {
        readonly IEnumerable<BenchComparative> _items;
        readonly int _iterations;
        readonly string _name;

        public BenchComparison(
            string name,
            IEnumerable<BenchComparative> items,
            int iterations)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(items, "items");

            _name = name;
            _items = items;
            _iterations = iterations;
        }

        public IEnumerable<BenchComparative> Items { get { return _items; } }

        public int Iterations { get { return _iterations; } }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
