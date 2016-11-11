// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    //using System.Collections.ObjectModel;

    public sealed class RetryPolicy
    {
        private readonly IList<Type> _retryableExceptionTypes;
        //private readonly Lazy<IReadOnlyCollection<Type>> _retryableExceptionTypesThunk;

        public RetryPolicy(int maxRetries, TimeSpan retryInterval, IList<Type> retryableExceptionTypes)
        {
            Require.NotNull(retryableExceptionTypes, nameof(retryableExceptionTypes));
            Require.Range(maxRetries > 0, nameof(maxRetries));
            Require.Range(retryInterval > TimeSpan.Zero, nameof(retryInterval));

            MaxRetries = maxRetries;
            RetryInterval = retryInterval;
            _retryableExceptionTypes = retryableExceptionTypes;
            //_retryableExceptionTypesThunk
            //    = new Lazy<IReadOnlyCollection<Type>>(
            //        () => new ReadOnlyCollection<Type>(retryableExceptionTypes));
        }

        //public IReadOnlyCollection<Type> RetryableExceptionTypes
        //    => _retryableExceptionTypesThunk.Value;

        public int MaxRetries { get; }

        public TimeSpan RetryInterval { get; }

        public bool MayRetryAfter(Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            return _retryableExceptionTypes.Contains(exception.GetType());
        }
    }
}
