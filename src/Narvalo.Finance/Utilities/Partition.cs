// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Spread? Partition? Evently?
    public enum Distribution
    {
        PseudoUniform,
        Single,
    }

    public enum MinorUnits
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
    }

    public enum Rounding
    {
        Floor,
        Ceiling,
        ToEven,
        AwayFromZero,
    }

    // Notes:
    // With a PCL, we can not use Decimal.Round, instead we have Math.Round.
    // Full .NET:
    //   int r;
    //   int low = Math.DivRem(value, n, out r);
    // We could have writen r = value % n
    //
    // - PEAA p.494
    // - Double.MaxValue > Decimal.MaxValue > Int64.MaxValue > Int32.MaxValue
    // -  Check if this is correct for negative values.
    //
    // Math.Floor (down), Math.Ceiling (up), Math.Truncate (zero), Math.Round?
    // - Floor = partie entière (n <= x < n +1); partie fractionnaire ({x} = x - |x|)
    // - Ceiling (n - 1 < x <= n)
    // - Truncate
    // - Round(ToEven)
    // - Round(AwayFromZero)
    // Remark: Integer division rounds toward zero.
    // - Explain how to reverse or randomize the distribution.
    public static class Partition
    {
        private static readonly Dictionary<int, decimal> s_Corrections = new Dictionary<int, decimal>
        {
            { 0, 0M },
            { 1, 0.1M },
            { 2, 0.01M },
            { 3, 0.001M },
            { 4, 0.0001M },
            { 5, 0.00001M },
        };

        // Evenly distribute an integer (value) across n integers:
        // value = nq + r = (n - r) q + r (q + 1)
        public static IEnumerable<int> Distribute(int value, int n)
        {
            var low = value / n;
            var high = low + 1;
            var r = value - low * n;

            // The high value will appear r times and the low value n - r times.
            for (var i = 0; i < n; i++)
            {
                yield return i < r ? high : low;
            }
        }

        /// <see cref="Distribute(int, int)">.
        public static IEnumerable<long> Distribute(long value, long n)
        {
            var low = value / n;
            var high = low + 1;
            var r = value - low * n;

            for (var i = 0; i < n; i++)
            {
                yield return i < r ? high : low;
            }
        }

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int minorUnits,
            int n,
            MidpointRounding mode,
            Distribution behaviour)

        {
            switch (behaviour)
            {
                case Distribution.PseudoUniform:
                    throw new NotImplementedException();
                case Distribution.Single:
                    return Distribute(value, minorUnits, n, mode).Reverse();
                default:
                    throw Check.Unreachable();
            }
        }

        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, int n)
            => Distribute(value, minorUnits, n, MidpointRounding.AwayFromZero);

        // NB: Highest value comes last and is not rounded.
        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, int n, MidpointRounding mode)
        {
            decimal q = Math.Round(value / n, minorUnits, mode);

            for (var i = 0; i < n - 1; i++)
            {
                yield return q;
            }

            yield return value - (n - 1) * q;
        }

        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, long[] ratios)
            => Distribute(value, minorUnits, ratios, MidpointRounding.AwayFromZero);

        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, long[] ratios, MidpointRounding mode)
        {
            var len = ratios.Length;
            var sum = ratios.Sum();
            decimal last = value;

            for (var i = 0; i < len - 1; i++)
            {
                var tmp = Math.Round(value * ratios[i] / sum, minorUnits, mode);
                last -= tmp;
                yield return tmp;
            }

            yield return last;
        }

        // http://www.timlabonne.com/2013/07/distributing-monetary-amounts-in-c/
        public static decimal[] Distribute(decimal value, long[] ratios)
        {
            var len = ratios.Length;

            var total = 0M;
            for (int i = 0; i < len; i++) { total += ratios[i]; }

            var remainder = value;
            var dist = new decimal[len];
            for (int i = 0; i < len; i++)
            {
                dist[i] = Math.Floor(value * ratios[i] / total);
                remainder -= dist[i];
            }

            for (int i = 0; i < remainder; i++)
            {
                dist[i]++;
            }

            return dist;
        }
    }
}
