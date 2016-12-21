// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Validation
{
    using System;

    [Flags]
    public enum IbanValidations
    {
        None = 0,

        Integrity = 1 << 0,

        ISOCountryCode = 1 << 1,

        Any = Integrity | ISOCountryCode,
    }

    public static class IbanValidationsExtensions
    {
        public static bool Contains(this IbanValidations @this, IbanValidations value) => (@this & value) != 0;
    }
}
