namespace Narvalo.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkAttribute : Attribute
    {
        string _displayName = String.Empty;
        int _iterations;

        public BenchmarkAttribute(int iterations)
        {
            _iterations = iterations;
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public int Iterations
        {
            get { return _iterations; }
        }
    }
}
