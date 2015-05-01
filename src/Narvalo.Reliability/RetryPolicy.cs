// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;

    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _retryInterval;
        private readonly IList<Type> _retryableExceptionTypes;
        private readonly Lazy<IReadOnlyCollection<Type>> _retryableExceptionTypesThunk;

        public RetryPolicy(int maxRetries, TimeSpan retryInterval, IList<Type> retryableExceptionTypes)
        {
            Require.GreaterThan(maxRetries, 0, "maxRetries");
            Require.NotNull(retryableExceptionTypes, "retryableExceptionTypes");
            Guard.GreaterThan(retryInterval, TimeSpan.Zero, "retryInterval");

            _maxRetries = maxRetries;
            _retryInterval = retryInterval;
            _retryableExceptionTypes = retryableExceptionTypes;
            _retryableExceptionTypesThunk
                = new Lazy<IReadOnlyCollection<Type>>(
                    () => new ReadOnlyCollection<Type>(retryableExceptionTypes));
        }

        public IReadOnlyCollection<Type> RetryableExceptionTypes
        {
            get { return _retryableExceptionTypesThunk.Value; }
        }

        public int MaxRetries { get { return _maxRetries; } }

        public TimeSpan RetryInterval { get { return _retryInterval; } }

        public bool MayRetryAfter(Exception exception)
        {
            Require.NotNull(exception, "exception");

            return _retryableExceptionTypes.Contains(exception.GetType());
        }
    }
}
