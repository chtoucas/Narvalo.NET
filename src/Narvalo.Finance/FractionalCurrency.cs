// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    /// <summary>
    /// Represents a subunit of a currency such as the EUR Cent or the GBP Penny.
    /// </summary>
    /// <remarks>
    /// <para>A subunit is a fraction of the main unit. The minor currency unit (when it exists)
    /// is such an example. It is possible for a currency to have more than one subunit.</para>
    /// <para>Contrary to a <see cref="Currency"/>, we do not restrict ourselves to subunits
    /// for which <see cref="Factor"/> is a power of 10.</para>
    /// </remarks>
    public partial struct FractionalCurrency : IEquatable<FractionalCurrency>
    {
        public FractionalCurrency(Currency parent, decimal epsilon, string code)
        {
            Require.Range(0m < epsilon && epsilon < 1m, nameof(epsilon));
            Require.NotNull(code, nameof(code));

            Parent = parent;
            Epsilon = epsilon;
            Code = code;
        }

        //public FractionalCurrency(Currency parent, int factor, string code)
        //{
        //    Require.Range(factor > 1, nameof(factor));
        //    Require.NotNull(code, nameof(code));

        //    Parent = parent;
        //    Epsilon = 1 / factor;
        //    Code = code;
        //}

        //internal FractionalCurrency(Currency parent, uint factor, string code)
        //{
        //    Require.True(factor > 1, nameof(factor));
        //    Require.NotNull(code, nameof(code));

        //    Parent = parent;
        //    Epsilon = 1 / factor;
        //    Code = code;
        //}

        public string Code { get; }

        /// <summary>
        /// Gets the smallest positive (non zero) value of the subunit expressed in the main unit.
        /// </summary>
        /// <example>For instance, EUR Cent: Epsilon = .01, that is 1 Cent is equal to .01 EUR.</example>
        public decimal Epsilon { get; }

        public Currency Parent { get; }

        public decimal Factor => 1 / Epsilon;

        public override string ToString() => Code;
    }

    // Interface IEquatable<FractionalCurrency>.
    public partial struct FractionalCurrency
    {
        public static bool operator ==(FractionalCurrency left, FractionalCurrency right) => left.Equals(right);

        public static bool operator !=(FractionalCurrency left, FractionalCurrency right) => !left.Equals(right);

        public bool Equals(FractionalCurrency other)
            => Parent == other.Parent
            && Code == other.Code
            && Epsilon == other.Epsilon;

        public override bool Equals(object obj)
        {
            if (!(obj is FractionalCurrency)) { return false; }

            return Equals((FractionalCurrency)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Parent.GetHashCode();
                hash = 31 * hash + Epsilon.GetHashCode();
                hash = 31 * hash + Code.GetHashCode();
                return hash;
            }
        }
    }
}
