// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Flags]
    public enum CurrencyTypes
    {
        Active = 1 << 0,

        Withdrawn = 1 << 1,

        Custom = 1 << 2,

        Current = Active | Custom,

        ISO = Active | Withdrawn,

        Any = Active | Withdrawn | Custom
    }

    public static class CurrencyTypesExtensions
    {
        public static bool Contains(this CurrencyTypes @this, CurrencyTypes value) => (@this & value) != 0;
    }
}
