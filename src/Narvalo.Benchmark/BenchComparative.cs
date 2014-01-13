namespace Narvalo.Benchmark
{
    using System;

    public class BenchComparative
    {
        readonly string _name;
        readonly Action _action;

        public BenchComparative(string name, Action action)
        {
            Requires.NotNullOrEmpty(name, "name");
            Requires.NotNull(action, "action");

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
