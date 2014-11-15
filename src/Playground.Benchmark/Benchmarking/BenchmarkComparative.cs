namespace Playground.Benchmark.Benchmarking
{
    using System;
    using Narvalo;

    public class BenchmarkComparative
    {
        readonly string _name;
        readonly Action _action;

        public BenchmarkComparative(string name, Action action)
        {
            Require.NotNullOrEmpty(name, "name");
            Require.NotNull(action, "action");

            _name = name;
            _action = action;
        }

        public Action Action { get { return _action; } }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
