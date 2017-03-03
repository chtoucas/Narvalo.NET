// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System;
    using System.Linq;

    public partial struct RatioArray : IEquatable<RatioArray>
    {
        private readonly decimal[] _ratios;

        private RatioArray(decimal[] ratios)
        {
            Demand.NotNull(ratios);

            _ratios = ratios;
        }

        public int Length => _ratios.Length;

        public decimal this[int index] => _ratios[index];

        public static RatioArray Of(decimal ratio)
        {
            var ratios = new decimal[2] { ratio, 1M - ratio };
            return new RatioArray(ratios);
        }

        public static RatioArray Of(decimal[] ratios)
        {
            Require.NotNull(ratios, nameof(ratios));

            if (ratios.Sum() != 1M) { throw new ArgumentException("XXX", nameof(ratios)); }

            return new RatioArray(ratios);
        }

        public static RatioArray FromPercentage(int percentage)
        {
            decimal ratio = percentage * 0.01M;

            return Of(ratio);
        }

        public static RatioArray FromPercentages(int[] percentages)
        {
            Require.NotNull(percentages, nameof(percentages));

            if (percentages.Sum() != 100) { throw new ArgumentException("XXX", nameof(percentages)); }

            var ratios = new decimal[percentages.Length];
            for (var i = 0; i < percentages.Length; i++)
            {
                ratios[i] *= 0.01M;
            }

            return new RatioArray(ratios);
        }
    }

    // Implements the IEquatable<Ratios> interface.
    public partial struct RatioArray
    {
        public static bool operator ==(RatioArray left, RatioArray right) => left.Equals(right);

        public static bool operator !=(RatioArray left, RatioArray right) => !left.Equals(right);

        public bool Equals(RatioArray other) => _ratios == other._ratios;

        public override bool Equals(object obj) => (obj is RatioArray) && Equals((RatioArray)obj);

        public override int GetHashCode() => _ratios.GetHashCode();
    }
}
