// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanStyles
    {
        None = 0,

        AllowLeadingWhite = 1 << 0,

        AllowTrailingWhite = 1 << 1,

        AllowInnerWhite = 1 << 2,

        AllowInnerHyphen = 1 << 3,

        AllowWhiteSpaces = AllowLeadingWhite | AllowTrailingWhite | AllowInnerWhite,
        Any = AllowWhiteSpaces | AllowInnerHyphen,
    }

    public static class IbanStylesExtensions
    {
        public static bool Contains(this IbanStyles @this, IbanStyles value) => (@this & value) != 0;
    }
}
