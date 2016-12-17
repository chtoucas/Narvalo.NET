// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum IbanStyles
    {
        None = 0,

        AllowWhiteSpaces = 1 << 0,

        AllowDashes = 1 << 1,

        Any = AllowWhiteSpaces | AllowDashes,
    }

    public static class IbanStylesExtensions
    {
        public static bool Contains(this IbanStyles @this, IbanStyles value) => (@this & value) != 0;
    }
}
