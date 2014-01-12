namespace Narvalo.Benchmarks
{
    using System;

    public class Benchmark
    {
        readonly int _iterations;
        readonly string _name;
        readonly Action _action;

        public Benchmark(string name, Action action, int iterations)
        {
            Requires.NotNullOrEmpty(name, "name");
            Requires.NotNull(action, "action");

            _name = name;
            _action = action;
            _iterations = iterations;
        }

        public Action Action { get { return _action; } }

        public int Iterations { get { return _iterations; } }

        public string Name { get { return _name; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
