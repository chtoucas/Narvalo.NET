// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using System.Diagnostics.Contracts;

    using NodaTime;

    [ContractClass(typeof(IBenchmarkTimerContract))]
    public interface IBenchmarkTimer
    {
        Duration ElapsedTime { get; }

        void Reset();
    }

    [ContractClassFor(typeof(IBenchmarkTimer))]
    internal abstract class IBenchmarkTimerContract : IBenchmarkTimer
    {
        Duration IBenchmarkTimer.ElapsedTime
        {
            get
            {
                Contract.Ensures(Contract.Result<Duration>().Ticks > 0L);

                return default(Duration);
            }
        }

        void IBenchmarkTimer.Reset()
        {
            // Intentionally left blank.
        }
    }
}
