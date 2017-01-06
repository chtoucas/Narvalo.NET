// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public interface IDecimalRounding
    {
        decimal Round(decimal value, int decimals);
    }
}
