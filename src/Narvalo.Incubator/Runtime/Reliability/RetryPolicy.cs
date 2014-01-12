namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Narvalo.Diagnostics;

    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _retryInterval;
        private readonly IList<Type> _retryableExceptionTypes;
        private readonly Lazy<ReadOnlyCollection<Type>> _retryableExceptionTypesThunk;

        public RetryPolicy(int maxRetries, TimeSpan retryInterval, IList<Type> retryableExceptionTypes)
        {
            Requires.GreaterThanOrEqualTo(maxRetries, 1, "maxRetries");
            // FIXME: la comparaison doit être stricte.
            Requires.GreaterThanOrEqualTo(retryInterval, TimeSpan.Zero, "retryInterval");
            Requires.NotNull(retryableExceptionTypes, "retryableExceptionTypes");

            _maxRetries = maxRetries;
            _retryInterval = retryInterval;
            _retryableExceptionTypes = retryableExceptionTypes;
            _retryableExceptionTypesThunk
                = new Lazy<ReadOnlyCollection<Type>>(
                    () => new ReadOnlyCollection<Type>(retryableExceptionTypes));
        }

        public ReadOnlyCollection<Type> RetryableExceptionTypes
        {
            get { return _retryableExceptionTypesThunk.Value; }
        }
        public int MaxRetries { get { return _maxRetries; } }
        public TimeSpan RetryInterval { get { return _retryInterval; } }

        public bool MayRetryAfter(Exception ex)
        {
            Requires.NotNull(ex, "ex");

            return _retryableExceptionTypes.Contains(ex.GetType());
        }
    }
}
