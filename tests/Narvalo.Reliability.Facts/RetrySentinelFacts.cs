// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public static partial class RetrySentinelFacts
    {
        private static readonly IList<Type> s_RetryableExceptions
            = new List<Type>(1) { typeof(RetryableException), };

        private static RetryPolicy NewRetryPolicy(int maxRetries)
        {
            return new RetryPolicy(maxRetries, TimeSpan.Zero, s_RetryableExceptions);
        }

        [Serializable]
        public class RetryableException : Exception
        {
            public RetryableException() : base() {; }

            public RetryableException(string message) : base(message) {; }

            public RetryableException(string message, Exception innerException)
                : base(message, innerException) {; }

            protected RetryableException(SerializationInfo info, StreamingContext context)
                : base(info, context) {; }
        }
    }
}
