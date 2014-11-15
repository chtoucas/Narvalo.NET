namespace Narvalo.Benchmarking
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BenchmarkComparativeAttribute : Attribute
    {
        string _displayName = String.Empty;

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
    }
}
