// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    public interface IMonitorableSentinel : IReliabilitySentinel
    {
        SentinelStatus Status { get; }
    }
}
