namespace Narvalo.Benchmark
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BenchComparativeAttribute : Attribute
    {
        string _displayName = String.Empty;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
