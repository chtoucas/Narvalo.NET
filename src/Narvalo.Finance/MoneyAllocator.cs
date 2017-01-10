// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    public sealed class /*Default*/MoneyAllocator : IMoneyAllocator
    {
        public IEnumerable<Money> Distribute(Money money, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = money.Currency;
            decimal total = money.Amount;

            decimal q = total / count;
            Money part = new Money(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency);
        }

        public IEnumerable<Money> Distribute(Money money, RatioArray ratios)
        {
            Currency currency = money.Currency;
            decimal total = money.Amount;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = ratios[i] * total;
                last -= amount;
                yield return new Money(amount, currency);
            }

            yield return new Money(last, currency);
        }
    }
}
