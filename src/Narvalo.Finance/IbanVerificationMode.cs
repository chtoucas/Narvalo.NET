// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanVerificationModes
    {
        Integrity = 1 << 0,

        Bban = 1 << 1,

        Any = Integrity | Bban,
    }
}
