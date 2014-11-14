namespace Narvalo.Runtime.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkComparisonAttribute : Attribute
    {
        string _displayName = String.Empty;
        int _iterations;

        public BenchmarkComparisonAttribute(int iterations)
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
