// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Extensions
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Allocators;
    using Narvalo.Finance.Rounding;

    // Allocation.
    public static partial class MoneyExtensions
    {
        private static readonly IMoneyAllocator s_DefaultAllocator = new DefaultMoneyAllocator();

        public static IEnumerable<Money> Allocate(this Money @this, int count)
            => s_DefaultAllocator.Allocate(@this, count);

        public static IEnumerable<Money> Allocate(this Money @this, int count, MidpointRounding mode)
            => new MidpointRoundingMoneyAllocator(mode).Allocate(@this, count);

        public static IEnumerable<Money> Allocate(this Money @this, int count, IRoundingAdjuster adjuster)
            => new RoundingMoneyAllocator(adjuster).Allocate(@this, count);

        public static IEnumerable<Money> Allocate(this Money @this, int count, IMoneyAllocator allocator)
        {
            Require.NotNull(allocator, nameof(allocator));
            return allocator.Allocate(@this, count);
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios)
            => s_DefaultAllocator.Allocate(@this, ratios);

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, MidpointRounding mode)
            => new MidpointRoundingMoneyAllocator(mode).Allocate(@this, ratios);

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, IRoundingAdjuster adjuster)
            => new RoundingMoneyAllocator(adjuster).Allocate(@this, ratios);

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, IMoneyAllocator allocator)
        {
            Require.NotNull(allocator, nameof(allocator));
            return allocator.Allocate(@this, ratios);
        }
    }

    // Standard binary math operators.
    public static partial class MoneyExtensions
    {
        public static Money Plus(this Money @this, decimal amount, MidpointRounding mode)
            => RoundingMachine.Add(@this, amount, mode);

        public static Money Plus(this Money @this, decimal amount, IRoundingAdjuster adjuster)
            => RoundingMachine.Add(@this, amount, adjuster);

        public static Money Plus(this Money @this, Money other, MidpointRounding mode)
            => RoundingMachine.Add(@this, other, mode);

        public static Money Plus(this Money @this, Money other, IRoundingAdjuster adjuster)
            => RoundingMachine.Add(@this, other, adjuster);

        public static Money Minus(this Money @this, decimal amount, MidpointRounding mode)
            => RoundingMachine.Subtract(@this, amount, mode);

        public static Money Minus(this Money @this, decimal amount, IRoundingAdjuster adjuster)
            => RoundingMachine.Subtract(@this, amount, adjuster);

        public static Money Minus(this Money @this, Money other, MidpointRounding mode)
            => RoundingMachine.Subtract(@this, other, mode);

        public static Money Minus(this Money @this, Money other, IRoundingAdjuster adjuster)
            => RoundingMachine.Subtract(@this, other, adjuster);

        public static Money MultiplyBy(this Money @this, decimal multiplier, MidpointRounding mode)
            => RoundingMachine.Multiply(@this, multiplier, mode);

        public static Money MultiplyBy(this Money @this, decimal multiplier, IRoundingAdjuster adjuster)
            => RoundingMachine.Multiply(@this, multiplier, adjuster);

        public static Money DivideBy(this Money @this, decimal divisor, MidpointRounding mode)
            => RoundingMachine.Divide(@this, divisor, mode);

        public static Money DivideBy(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
            => RoundingMachine.Divide(@this, divisor, adjuster);

        public static Money Mod(this Money @this, decimal divisor, MidpointRounding mode)
            => RoundingMachine.Remainder(@this, divisor, mode);

        public static Money Mod(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
            => RoundingMachine.Remainder(@this, divisor, adjuster);
    }
}