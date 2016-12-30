// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Linq;

    public partial struct Ratios : IEquatable<Ratios>
    {
        private readonly decimal[] _ratios;

        private Ratios(decimal[] ratios)
        {
            Demand.NotNull(ratios);

            _ratios = ratios;
        }

        public int Length => _ratios.Length;

        public decimal this[int index] => _ratios[index];

        public static Ratios Of(decimal ratio)
        {
            var ratios = new decimal[2] { ratio, 1M - ratio };
            return new Ratios(ratios);
        }

        public static Ratios Of(decimal[] ratios)
        {
            Require.NotNull(ratios, nameof(ratios));

            if (ratios.Sum() != 1M) { throw new ArgumentException("XXX", nameof(ratios)); }

            return new Ratios(ratios);
        }

        public static Ratios FromPercentage(int percentage)
        {
            decimal ratio = percentage * 0.01M;

            return Of(ratio);
        }

        public static Ratios FromPercentages(int[] percentages)
        {
            Require.NotNull(percentages, nameof(percentages));

            if (percentages.Sum() != 100) { throw new ArgumentException("XXX", nameof(percentages)); }

            var ratios = new decimal[percentages.Length];
            for (var i = 0; i < percentages.Length; i++)
            {
                ratios[i] *= 0.01M;
            }

            return new Ratios(ratios);
        }
    }

    // Implements the IEquatable<Ratios> interface.
    public partial struct Ratios
    {
        public static bool operator ==(Ratios left, Ratios right) => left.Equals(right);

        public static bool operator !=(Ratios left, Ratios right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Ratios other) => _ratios == other._ratios;

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            if (!(obj is Ratios)) { return false; }

            return Equals((Ratios)obj);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => _ratios.GetHashCode();
    }
}
