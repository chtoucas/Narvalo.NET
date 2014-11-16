namespace Narvalo.Benchmarking
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BenchmarkException : Exception
    {
        public BenchmarkException() : base() { }

        public BenchmarkException(string message) : base(message) { }

        public BenchmarkException(string message, Exception innerException)
            : base(message, innerException) { }

        protected BenchmarkException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
