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

        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite | AllowInnerWhite,

        Any = AllowWhiteSpaces | AllowHeader | AllowLowercaseLetter,
    }

    public static class IbanStylesExtensions
    {
        // WARNING: This always returns false for IbanStyles.None.
        public static bool Contains(this IbanStyles @this, IbanStyles value) => (@this & value) != 0;
    }
}
