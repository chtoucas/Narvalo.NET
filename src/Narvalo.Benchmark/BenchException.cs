namespace Narvalo.Benchmark
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BenchException : Exception
    {
        public BenchException() : base() { ; }

        public BenchException(string message) : base(message) { ; }

        public BenchException(string message, Exception innerException)
            : base(message, innerException) { ; }

        protected BenchException(SerializationInfo info, StreamingContext context)
            : base(info, context) { ; }
    }
}
