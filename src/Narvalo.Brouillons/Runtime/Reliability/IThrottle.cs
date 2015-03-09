// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    public interface IThrottle : IBarrier
    {
        bool IsConstricted { get; }

        bool IsObstructed { get; }
    }
}
