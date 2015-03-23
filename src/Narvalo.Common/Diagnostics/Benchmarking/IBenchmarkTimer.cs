// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
#if CONTRACTS_FULL // [Intentionally] Using directive.
    using System.Diagnostics.Contracts;
#endif

    public partial interface IBenchmarkTimer
    {
        TimeSpan ElapsedTime { get; }

        void Reset();
    }

#if CONTRACTS_FULL // [Ignore] Contract Class and Object Invariants.

    [ContractClass(typeof(IBenchmarkTimerContract))]
    public partial interface IBenchmarkTimer { }

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

#endif
}
