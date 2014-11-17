namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    // Method injection
    // http://www.codeproject.com/Articles/37549/CLR-Injection-Runtime-Method-Replacer
    // http://blog.stevensanderson.com/2009/08/24/writing-great-unit-tests-best-and-worst-practises/

    public partial class CircuitBreakerFacts
    {
        static readonly IList<Type> RetryableExceptions
           = new List<Type>(1) { typeof(RetryableException), };

        static CircuitBreaker NewCircuitBreaker()
        {
            return new CircuitBreaker(1, TimeSpan.Zero);
        }

        static CircuitBreaker NewCircuitBreaker(int threshold)
        {
            return new CircuitBreaker(threshold, TimeSpan.MaxValue);
        }

        static RetryPolicy NewRetryPolicy(int maxRetries)
        {
            return new RetryPolicy(maxRetries, TimeSpan.Zero, RetryableExceptions);
        }

        [Serializable]
        public class RetryableException : Exception
        {
            public RetryableException() : base() { ; }

            public RetryableException(string message) : base(message) { ; }

            public RetryableException(string message, Exception innerException)
                : base(message, innerException) { ; }

            protected RetryableException(SerializationInfo info, StreamingContext context)
                : base(info, context) { ; }
        }
    }
}
