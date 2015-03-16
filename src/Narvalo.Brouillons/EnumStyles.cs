// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    [Flags]
    [Serializable]
    public enum EnumStyles
    {
        None = 0,

        Literal = 1 << 0,

        Integer = 1 << 1,

        AllowLeadingWhite = 1 << 2,

        AllowTrailingWhite = 1 << 3,

        Any = Literal | Integer | AllowWhiteSpaces,

        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite,
    }
}
