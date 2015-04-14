// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon
{
    using System;

    public partial interface IBenchmarkTimer
    {
        TimeSpan ElapsedTime { get; }

        void Reset();
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Benchmarking
{
    using System;
    using System.Diagnostics.Contracts;

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

        public abstract void Reset();
    }
}

#endif
