// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    public interface IRoundingAdjuster
    {
        decimal Round(decimal value);

        decimal Round(decimal value, int decimalPlaces);
    }
}
