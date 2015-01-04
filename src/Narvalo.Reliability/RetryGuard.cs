// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class RetryGuard : IGuard
    {
        readonly RetryPolicy _policy;

        public RetryGuard(RetryPolicy policy)
        {
            Require.NotNull(policy, "policy");

            _policy = policy;
        }

        public int MaxTries { get { return 1 + _policy.MaxRetries; } }

        public RetryPolicy Policy { get { return _policy; } }

        #region IGuard

        public void Execute(Action action)
        {
            Require.NotNull(action, "action");

            int attempts = 0;
            var exceptions = new List<Exception>();

            while (++attempts <= MaxTries) {
                try {
                    action();
                    break;
                }
                catch (GuardException) {
                    throw NewAggregateGuardException("TODO", exceptions);
                }
                catch (Exception ex) {
                    exceptions.Add(ex);

                    if (!_policy.MayRetryAfter(ex)) {
                        throw NewAggregateGuardException("TODO", exceptions);
                    }

                    // On n'attend pas si on a déjà atteint la limite d'essais acceptés.
                    if (attempts < MaxTries) {
                        //Thread.Sleep(_retryInterval);
                        Wait(_policy.RetryInterval);
                    }
                }
            }
        }

        #endregion

        static AggregateGuardException NewAggregateGuardException(
           string message, IList<Exception> exceptions)
        {
            if (exceptions.Count > 0) {
                return new AggregateGuardException("TODO", new AggregateException(exceptions));
            }
            else {
                return new AggregateGuardException("TODO");
            }
        }

        static void Wait(TimeSpan duration)
        {
            using (var resetEvent = new ManualResetEvent(false /* initialState */)) {
                TimerCallback cb = (state) => { (state as ManualResetEvent).Set(); };
                // FIXME var dueTime = (long)duration.TotalMilliseconds;
                var dueTime = (int)duration.TotalMilliseconds;

                using (var tmr = new Timer(cb, resetEvent, dueTime, Timeout.Infinite)) {
                    resetEvent.WaitOne();
                }
            }
        }
    }
}
