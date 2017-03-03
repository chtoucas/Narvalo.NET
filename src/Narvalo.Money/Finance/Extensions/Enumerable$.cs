// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Extensions
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Rounding;

    // LINQ operators.
    public static class EnumerableExtensions
    {
        public static Money Average(this IEnumerable<Money> @this)
            => MoneyCalculator.Average(@this);

        public static Money Average(this IEnumerable<Money> @this, MidpointRounding mode)
            => RoundingMachine.Average(@this, mode);

        public static Money Average(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
            => RoundingMachine.Average(@this, adjuster);

        public static Money? Average(this IEnumerable<Money?> @this)
            => MoneyCalculator.Average(@this);

        public static Money? Average(this IEnumerable<Money?> @this, MidpointRounding mode)
            => RoundingMachine.Average(@this, mode);

        public static Money? Average(this IEnumerable<Money?> @this, IRoundingAdjuster adjuster)
            => RoundingMachine.Average(@this, adjuster);

        public static Money Sum(this IEnumerable<Money> @this)
            => MoneyCalculator.Sum(@this);

        public static Money Sum(this IEnumerable<Money> @this, MidpointRounding mode)
            => RoundingMachine.Sum(@this, mode);

        public static Money Sum(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
            => RoundingMachine.Sum(@this, adjuster);

        public static Money Sum(this IEnumerable<Money?> @this)
            => MoneyCalculator.Sum(@this);

        public static Money Sum(this IEnumerable<Money?> @this, MidpointRounding mode)
            => RoundingMachine.Sum(@this, mode);

        public static Money Sum(this IEnumerable<Money?> @this, IRoundingAdjuster adjuster)
            => RoundingMachine.Sum(@this, adjuster);
    }
}
