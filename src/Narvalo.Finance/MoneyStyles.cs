// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// Defines the options that customized the behaviour of the Parse and TryParse methods
    /// of the Money type.
    /// </summary>
    [Flags]
    public enum MoneyStyles
    {
        None = 0,

        AllowLeadingWhite = 1 << 0,

        AllowTrailingWhite = 1 << 1,

        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite,

        Any = AllowWhiteSpaces,
    }

    public static class MoneyStylesExtensions
    {
        // WARNING: This always returns false for MoneyStyles.None.
        public static bool Contains(this MoneyStyles @this, MoneyStyles styles) => (@this & styles) != 0;
    }
}
