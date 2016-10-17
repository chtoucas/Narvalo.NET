// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    public enum SentinelStatus
    {
        Busy,

        // Unrecoverable: we're basically dead for good.
        Down,

        // Currently unavailable.
        TemporarilyDown,

        // Something's wrong, but we can still serve requests. (Degraded)
        Degraded,

        // Everything is operating as expected.
        Up,
    }
}
