// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public enum MinorUnits
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
    }

    // See https://en.wikipedia.org/wiki/Rounding
    // In .NET:
    // - ToEven, aka Bankers' Rounding.
    //   Advantage: it is symmetric (negative and positive numbers are treated equally);
    //   we can divide the result by two without losing precision; it is unbalanced compare to Down
    //  or Up when it comes to cumulative computations followed by approximations.
    // - AwayFromZero,
    // Stochastic?
    public enum NumberRounding
    {
        Down,
        Up,
        TowardZero,
        AwayFromZero,
        Nearest,
    }

    // Spread? Partition? Evenly?
    //public enum Distribution
    //{
    //    PseudoUniform,
    //    Single,
    //}

    public static class DivisionResult
    {
        public static DivisionResult<T> Create<T>(T value, int n, T remainder)
            => new DivisionResult<T>(value, n, remainder);
    }

    public sealed class DivisionResult<T>
    {
        private readonly IEnumerable<T> _coll;
        private readonly T _remainder;

        public DivisionResult(T value, int n, T remainder)
        {
            _coll = Enumerable.Repeat(value, n);
            _remainder = remainder;
        }

        public T Remainder => _remainder;

        public IEnumerable<T> ToEnumerable() => _coll;
    }

    public sealed class Weights
    {
        private readonly IEnumerable<decimal> _weights;

        private Weights(IEnumerable<decimal> weights)
        {
            _weights = weights;
        }

        public IEnumerable<decimal> AsEnumerable() => _weights;

        public static Weights NewPartition(IEnumerable<int> partition)
        {
            var arr = partition.ToArray();
            if (arr.Sum() != 100) { throw new ArgumentException(); }

            return new Weights(from _ in partition select _ * 0.01M);
        }

        public static Weights NewPartition(IEnumerable<decimal> ratios)
        {
            var arr = ratios.ToArray();
            if (ratios.Sum() != 1M) { throw new ArgumentException(); }

            return new Weights(ratios);
        }
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
    public static class MathOps
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

        public static DivisionResult<int> Divide(int value, int n)
        {
            var low = value / n;
            var remainder = value - low * n;

            return DivisionResult.Create(low, n, remainder);
        }

        public static DivisionResult<long> Divide(long value, int n)
        {
            var low = value / n;
            var remainder = value - low * n;

            return DivisionResult.Create(low, n, remainder);
        }

        // Distribute an integer (value) across n copies of value / n:
        //   value = nq + r = (n - r) q + r (q + 1) where q = value / n.
        // First returns the high value r times, then the low value n - r times.
        public static IEnumerable<int> Distribute(int value, int n)
        {
            var low = value / n;
            var high = low + 1;
            var r = value - low * n;

            for (var i = 0; i < n; i++)
            {
                yield return i < r ? high : low;
            }
        }

        /// <see cref="Divide(int, int)">.
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

        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, int n)
            => Distribute(value, minorUnits, n, MidpointRounding.ToEven);

        public static IEnumerable<decimal> Distribute(decimal value, int minorUnits, int n, MidpointRounding mode)
        {
            decimal q = Math.Round(value / n, minorUnits, mode);

            for (var i = 0; i < n - 1; i++)
            {
                yield return q;
            }

            var last = value - (n - 1) * q;

            yield return last;
        }

        public static IEnumerable<decimal> Allocate(
            decimal value,
            int minorUnits,
            Weights weights,
            MidpointRounding mode)
        {
            var coll = from _ in weights.AsEnumerable() select Math.Round(_ * value, minorUnits, mode);
            var arr = coll.ToArray();
            arr[arr.Length - 1] = value - (arr.Sum() - arr[arr.Length - 1]);

            return arr;
        }

        public static IEnumerable<decimal> Allocate(decimal value, int minorUnits, int[] partition)
            => Allocate(value, minorUnits, partition, MidpointRounding.ToEven);

        public static IEnumerable<decimal> Allocate(decimal value, int minorUnits, int[] partition, MidpointRounding mode)
        {
            var len = partition.Length;
            var sum = partition.Sum();
            var last = value;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = Math.Round(value * partition[i] / sum, minorUnits, mode);
                last -= next;
                yield return next;
            }

            yield return last;
        }
    }
}
