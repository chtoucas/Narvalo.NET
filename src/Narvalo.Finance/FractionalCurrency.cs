// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// Represents a subunit of a currency such as the EUR Cent or the GBP Penny.
    /// </summary>
    /// <remarks>A subunit is a fraction of the main unit. The minor currency unit (when it exists)
    /// is an example of a subunit, but a currency can have more than one subunit.</remarks>
    public partial struct FractionalCurrency : IEquatable<FractionalCurrency>
    {
        public FractionalCurrency(Currency parent, decimal subunit, string code)
        {
            Require.Range(subunit < 1m, nameof(subunit));

            Parent = parent;
            Subunit = subunit;
            Code = code;
        }

        public string Code { get; }

        public decimal Subunit { get; }

        public Currency Parent { get; }

        public decimal Factor => 1 / Subunit;
    }

    // Interface IEquatable<FractionalCurrency>.
    public partial struct FractionalCurrency
    {
        public static bool operator ==(FractionalCurrency left, FractionalCurrency right) => left.Equals(right);

        public static bool operator !=(FractionalCurrency left, FractionalCurrency right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(FractionalCurrency other)
            => Parent == other.Parent
            && Code == other.Code
            && Subunit == other.Subunit;

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            if (!(obj is FractionalCurrency)) { return false; }

            return Equals((FractionalCurrency)obj);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Parent.GetHashCode();
                hash = 31 * hash + Code.GetHashCode();
                hash = 31 * hash + Subunit.GetHashCode();
                return hash;
            }
        }
    }
}
