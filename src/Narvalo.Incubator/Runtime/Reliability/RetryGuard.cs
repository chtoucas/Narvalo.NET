namespace Narvalo.Runtime.Reliability
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	public class RetryGuard : IGuard
	{
		readonly RetryPolicy _policy;

		public RetryGuard(RetryPolicy policy)
		{
			Requires.NotNull(policy, "policy");

			_policy = policy;
		}

		public int MaxTries { get { return 1 + _policy.MaxRetries; } }

		public RetryPolicy Policy { get { return _policy; } }

		#region IGuard

		public void Execute(Action action)
		{
			Requires.NotNull(action, "action");

			int attempts = 0;
			var exceptions = new List<Exception>();

			while (++attempts <= MaxTries) {
				try {
					action();
					break;
				}
				catch (GuardException) {
					throw NewAggregateGuardException("XXX", exceptions);
				}
				catch (Exception ex) {
					exceptions.Add(ex);

					if (!_policy.MayRetryAfter(ex)) {
						throw NewAggregateGuardException("XXX", exceptions);
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

		private static AggregateGuardException NewAggregateGuardException(
			string message, IList<Exception> exceptions)
		{
			if (exceptions.Count > 0) {
				return new AggregateGuardException("XXX", new AggregateException(exceptions));
			}
			else {
				return new AggregateGuardException("XXX");
			}
		}

		private static void Wait(TimeSpan duration)
		{
			using (var resetEvent = new ManualResetEvent(false /* initialState */)) {
				TimerCallback cb = (state) => { (state as ManualResetEvent).Set(); };
				var dueTime = (long)duration.TotalMilliseconds;
				using (var tmr = new Timer(cb, resetEvent, dueTime, Timeout.Infinite)) {
					resetEvent.WaitOne();
				}
			}
		}
	}
}
