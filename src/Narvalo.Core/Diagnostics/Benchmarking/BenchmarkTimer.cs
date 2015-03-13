// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public sealed class BenchmarkTimer : IBenchmarkTimer
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public TimeSpan ElapsedTime
        {
            get
            {
                var result = TimeSpan.FromTicks(_stopwatch.Elapsed.Ticks);

                Contract.Assume(result.Ticks > 0L);

                return result;
            }
        }

        public void Reset()
        {
            _stopwatch.Reset(); 
            _stopwatch.Start();
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        [Conditional("CONTRACTS_FULL")]
        private void ObjectInvariants()
        {
            Contract.Invariant(_stopwatch != null);
        }

#endif
    }
}
