// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.BenchmarkCommon
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    public sealed class BenchmarkTimer : IBenchmarkTimer
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public TimeSpan ElapsedTime
        {
            get
            {
                var retval = TimeSpan.FromTicks(_stopwatch.Elapsed.Ticks);

                Contract.Assume(retval.Ticks > 0L, "'result.Ticks' is negative.");

                return retval;
            }
        }

        public void Reset()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(_stopwatch != null);
        }

#endif
    }
}
