// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    public partial struct Money
    {
        public static Money Create(decimal amount) => new Money(amount, Currency.Of("XXX"));

        public static Money Create(long amount, Currency currency) => new Money(amount, currency);

        [CLSCompliant(false)]
        public static Money Create(ulong amount, Currency currency) => new Money(amount, currency);
    }
}
