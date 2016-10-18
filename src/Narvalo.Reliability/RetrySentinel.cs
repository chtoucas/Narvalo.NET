// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class RetrySentinel : IReliabilitySentinel
    {
        public RetrySentinel(RetryPolicy policy)
        {
            Require.NotNull(policy, nameof(policy));

            Policy = policy;
        }

        public int MaxTries => 1 + Policy.MaxRetries;

        public RetryPolicy Policy { get; }

        public void Invoke(Action action)
        {
            Require.NotNull(action, nameof(action));

            int attempts = 0;
            var exceptions = new List<Exception>();

            while (++attempts <= MaxTries)
            {
                try
                {
                    action.Invoke();
                    break;
                }
                catch (ReliabilityException)
                {
                    throw CreateAggregateException("XXX", exceptions);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);

                    if (!Policy.MayRetryAfter(ex))
                    {
                        throw CreateAggregateException("XXX", exceptions);
                    }

                    // On n'attend pas si on a déjà atteint la limite d'essais acceptés.
                    if (attempts < MaxTries)
                    {
                        ////Thread.Sleep(_retryInterval);
                        Wait(Policy.RetryInterval);
                    }
                }
            }
        }

        private static AggregateReliabilityException CreateAggregateException(
           string message,
           IList<Exception> exceptions)
        {
            if (exceptions.Count > 0)
            {
                return new AggregateReliabilityException(message, new AggregateException(exceptions));
            }
            else
            {
                return new AggregateReliabilityException(message);
            }
        }

        private static void Wait(TimeSpan duration)
        {
            using (var resetEvent = new ManualResetEvent(false /* initialState */))
            {
                TimerCallback cb = (state) => { (state as ManualResetEvent).Set(); };

                // FIXME: var dueTime = (long)duration.TotalMilliseconds;
                var dueTime = (int)duration.TotalMilliseconds;

                using (var tmr = new Timer(cb, resetEvent, dueTime, Timeout.Infinite))
                {
                    resetEvent.WaitOne();
                }
            }
        }
    }
}
