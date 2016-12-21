// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanVerificationLevels
    {
        None = 0,

        Integrity = 1 << 0,

        ISOCountryCode = 1 << 1,

        Any = Integrity | ISOCountryCode,
    }

    public static class IbanVerificationLevelsExtensions
    {
        public static bool Contains(this IbanVerificationLevels @this, IbanVerificationLevels value)
            => (@this & value) != 0;
    }
}
