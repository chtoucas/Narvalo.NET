// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Internal;

    public static class PennyAllocators
    {
        public static IEnumerable<Moneypenny> Allocate(Moneypenny penny, int count)
        {
            Require.Range(count > 1, nameof(count));

            long q = penny.Amount / count;
            var part = new Moneypenny(q, penny.Currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Moneypenny(penny.Amount - (count - 1) * q, penny.Currency);
        }

        public static IEnumerable<Moneypenny> AllocateEvenly(Moneypenny penny, int count)
        {
            Require.Range(count > 1, nameof(count));

            var cy = penny.Currency;

            return from amount in Number.DivideEvenly(penny.Amount, count) select new Moneypenny(amount, cy);
        }

        //public static IEnumerable<Moneypenny> Allocate(Moneypenny penny, RatioArray ratios)
        //{
        //    Currency currency = penny.Currency;
        //    long total = penny.Amount;

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
