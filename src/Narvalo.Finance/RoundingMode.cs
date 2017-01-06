// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    public enum RoundingMode
    {
        None,

        Unnecessary,

        ToEven,

        AwayFromZero,

        Default = ToEven,
    }
}
