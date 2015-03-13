// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IBenchmarkTimerContract))]
    public interface IBenchmarkTimer
    {
        TimeSpan ElapsedTime { get; }

        void Reset();
    }

    [ContractClassFor(typeof(IBenchmarkTimer))]
    internal abstract class IBenchmarkTimerContract : IBenchmarkTimer
    {
        TimeSpan IBenchmarkTimer.ElapsedTime
        {
            get
            {
                Contract.Ensures(Contract.Result<TimeSpan>().Ticks > 0L);

                return default(TimeSpan);
            }
        }

        void IBenchmarkTimer.Reset() { }
    }
}
