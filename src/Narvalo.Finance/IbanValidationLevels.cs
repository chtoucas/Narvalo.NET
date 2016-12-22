// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanValidationLevels
    {
        None = 0,

        Integrity = 1 << 0,

        ISOCountryCode = 1 << 1,

        Bban = 1 << 2,

        Any = Integrity | ISOCountryCode | Bban,

        // NB: We do not enable country code verification per default; it seems
        // that the IBAN registry does not always use valid ISO alpha-2 codes.
        Default = Integrity,
    }

    public static class IbanVerificationLevelsExtensions
    {
        public static bool Contains(this IbanValidationLevels @this, IbanValidationLevels value)
            => (@this & value) != 0;
    }
}
