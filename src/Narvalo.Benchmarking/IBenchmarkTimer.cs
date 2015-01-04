// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Benchmarking
{
    using NodaTime;

    public interface IBenchmarkTimer
    {
        Duration ElapsedTime { get; }

        void Reset();
    }
}
