// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Finance.Utilities;

    public sealed class PennyAllocator
    {
        public IEnumerable<Moneypenny> Allocate(Moneypenny money, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Moneypenny>>();

            Currency currency = money.Currency;
            long total = money.Amount;

            long q = total / count;
            var part = new Moneypenny(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Moneypenny(total - (count - 1) * q, currency);
        }

        public IEnumerable<Moneypenny> AllocateEvenly(Moneypenny penny, long count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Moneypenny>>();

            return from _ in Integer.DistributeEvenly(penny.Amount, count)
                   select new Moneypenny(_, penny.Currency);
        }

        //public IEnumerable<Moneypenny> Allocate(Moneypenny money, RatioArray ratios)
        //{
        //    Currency currency = money.Currency;
        //    long total = money.Amount;

        //    int len = ratios.Length;
        //    var dist = new decimal[len];
        //    long last = total;

        //    for (var i = 0; i < len - 1; i++)
        //    {
        //        long amount = ratios[i] * total;
        //        last -= amount;
        //        yield return new Moneypenny(amount, currency);
        //    }

        //    yield return new Moneypenny(last, currency);
        //}
    }
}
