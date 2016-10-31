// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;

    using static Narvalo.Finance.Internal.Helpers;

    /// <summary>
    /// Represents an International Bank Account Number (IBAN).
    /// </summary>
    /// <remarks>
    /// The standard format for an IBAN is defined in ISO 13616.
    /// </remarks>
    public partial struct Iban : IEquatable<Iban>
    {
        private readonly string _bban;
        private readonly string _checkDigit;
        private readonly string _countryCode;
        private readonly string _value;

        //public Iban(string countryCode, string checkDigit, string bban)
        //    : this(countryCode, checkDigit, bban, countryCode + checkDigit + bban) { }

        private Iban(string countryCode, string checkDigit, string bban, string value)
        {
            Contract.Requires(countryCode != null);
            Contract.Requires(checkDigit != null);
            Contract.Requires(bban != null);
            Contract.Requires(value != null);

            _countryCode = countryCode;
            _checkDigit = checkDigit;
            _bban = bban;
            _value = value;
        }

        /// <summary>
        /// Gets the Basic Bank Account Number (BBAN).
        /// </summary>
        public string Bban
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _bban;
            }
        }

        /// <summary>
        /// Gets the check digit.
        /// </summary>
        public string CheckDigit
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _checkDigit;
            }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string CountryCode
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return _countryCode;
            }
        }

        public static Iban Parse(string value)
        {
            Require.NotNull(value, nameof(value));

            Iban? iban = ParseCore(value, true /* throwOnError */);

            Contract.Assume(iban.HasValue);

            return iban.Value;
        }

        public static Iban? TryParse(string value)
        {
            if (value == null)
            {
                return null;
            }

            return ParseCore(value, false /* throwOnError */);
        }

        public override string ToString() => _value;

        private static Iban? ParseCore(string value, bool throwOnError)
        {
            Contract.Requires(value != null);

            if (value.Length < 14 || value.Length > 34)
            {
                if (throwOnError)
                {
                    throw new ArgumentException(
                        "The IBAN string MUST be at most 34 characters long and at least 14 characters long.",
                        nameof(value));
                }
                else
                {
                    return null;
                }
            }

            for (int i = 0; i < value.Length; i++)
            {
                var pos = (int)value[i];
                bool valid = IsDigit(pos) || IsUpperLetter(pos);

                if (!valid)
                {
                    if (throwOnError)
                    {
                        throw new ArgumentException(
                            "The IBAN string MUST only contains digits and ASCII uppercase letters.",
                            nameof(value));
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            // The first two letters define the ISO 3166-1 alpha-2 country code.
            string countryCode = value.Substring(0, 2);

            string checkDigit = value.Substring(2, 2);

            string bban = value.Substring(4);

            return new Iban(countryCode, checkDigit, bban, value);
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_bban != null);
            Contract.Invariant(_checkDigit!= null);
            Contract.Invariant(_countryCode != null);
            Contract.Invariant(_value != null);
        }

#endif
    }

    // Implements the IEquatable<Iban> interface.
    public partial struct Iban
    {
        public static bool operator ==(Iban left, Iban right) => left.Equals(right);

        public static bool operator !=(Iban left, Iban right) => !left.Equals(right);

        public bool Equals(Iban other) => _value == other._value;

        public override bool Equals(object obj)
        {
            if (!(obj is Iban))
            {
                return false;
            }

            return Equals((Iban)obj);
        }

        public override int GetHashCode() => _value.GetHashCode();
    }
}
