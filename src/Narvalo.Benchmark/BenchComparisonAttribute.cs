namespace Narvalo.Benchmark
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BenchComparisonAttribute : Attribute
    {
        string _displayName = String.Empty;
        int _iterations;

        public BenchComparisonAttribute(int iterations)
        {
            _iterations = iterations;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int Iterations { get { return _iterations; } }
    }
}
