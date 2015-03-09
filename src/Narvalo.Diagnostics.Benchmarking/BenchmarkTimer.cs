// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Diagnostics.Benchmarking
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using NodaTime;

    public sealed class BenchmarkTimer : IBenchmarkTimer
    {
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public Duration ElapsedTime
        {
            get
            {
                var result = Duration.FromTicks(_stopwatch.Elapsed.Ticks);

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
        private void ObjectInvariants()
        {
            Contract.Invariant(_stopwatch != null);
        }

#endif
    }
}
