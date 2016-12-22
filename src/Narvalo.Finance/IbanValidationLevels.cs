// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanValidationLevels
    {
        None = 0,

        Integrity = 1 << 0,

        // NB: We might have to introduce an alternate country code verification. Indeed, it seems
        // that the IBAN registry does not always use valid ISO alpha-2 codes.
        ISOCountryCode = 1 << 1,

        Bban = 1 << 2,

        Any = Integrity | ISOCountryCode | Bban,

        Default = Integrity,
    }

    public static class IbanVerificationLevelsExtensions
    {
        public static bool Contains(this IbanValidationLevels @this, IbanValidationLevels value)
            => (@this & value) != 0;
    }
}
