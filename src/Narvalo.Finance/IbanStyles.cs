// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// Defines the options that customized the behaviour of the Parse and TryParse methods
    /// of the Iban type.
    /// </summary>
    [Flags]
    public enum IbanStyles
    {
        None = 0,

        AllowLeadingWhite = 1 << 0,

        AllowTrailingWhite = 1 << 1,

        AllowInnerWhite = 1 << 2,

        AllowHeader = 1 << 3,

        AllowLowercaseLetter = 1 << 4,

        CheckIntegrity = 1 << 5,

        CheckISOCountryCode = 1 << 6,

        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite | AllowInnerWhite,

        FullCheck = CheckIntegrity | CheckISOCountryCode,

        Any = AllowWhiteSpaces | AllowHeader | AllowLowercaseLetter | FullCheck,
    }

    public static class IbanStylesExtensions
    {
        public static bool Contains(this IbanStyles @this, IbanStyles value) => (@this & value) != 0;
    }
}
