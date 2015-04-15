// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    public interface IMonitorableGuard : ISentinel
    {
        GuardStatus Status { get; }
    }
}
